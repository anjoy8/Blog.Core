/*
 * Copyright 2014 Splunk, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"): you may
 * not use this file except in compliance with the License. You may obtain
 * a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */

using Serilog;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
 
namespace Blog.Core.Serilog.Es.Sinks.TCP
{
    /// <summary>
    /// TcpSocketWriter encapsulates queueing strings to be written to a TCP _socket
    /// and handling reconnections (according to a TcpConnectionPolicy object passed
    /// to it) when a TCP session drops.
    /// </summary>
    /// <remarks>
    /// TcpSocketWriter maintains a fixed sized queue of strings to be sent via
    /// the TCP _port and, while the _socket is open, sends them as quickly as possible.
    ///
    /// If the TCP session drops, TcpSocketWriter will stop pulling strings off the
    /// queue until it can reestablish a connection. Any SocketErrors emitted during this
    /// process will be passed as arguments to invocations of LoggingFailureHandler.
    /// If the TcpConnectionPolicy.Connect method throws an exception (in particular,
    /// TcpReconnectFailure to indicate that the policy has reached a point where it
    /// will no longer try to establish a connection) then the LoggingFailureHandler
    /// event is invoked, and no further attempt to log anything will be made.
    /// </remarks>
    public class TcpSocketWriter : IDisposable
    {
        private readonly FixedSizeQueue<string> _eventQueue;
        private readonly ExponentialBackoffTcpReconnectionPolicy _reconnectPolicy = new ExponentialBackoffTcpReconnectionPolicy();
        private readonly CancellationTokenSource _tokenSource; // Must be private or Dispose will not function properly.
        private readonly TaskCompletionSource<bool> _disposed = new TaskCompletionSource<bool>();

        private Stream _stream;

        /// <summary>
        /// Event that is invoked when reconnecting after a TCP session is dropped fails.
        /// </summary>
        public event Action<Exception> LoggingFailureHandler = ex =>
        {
            UnexpectedErrorLogger(
                ex, 
                (x, socketError) =>
                {
                    if (socketError == null)
                    {
                        //Log.Error(x, "failure inside TCP socket: {message}", x.Message);
                    }
                    else
                    {
                        //Log.Error(
                        //    x,
                        //    "failure inside TCP socket: {message} - socket error found {socketErrorCode}",
                        //    x.Message,
                        //    socketError);
                    }
                    
                });
        };

        public static void UnexpectedErrorLogger(Exception ex, Action<Exception, SocketError?> log)
        {
            SocketError? socketErrorCode = null;
            var current = ex;
            do
            {
                if (current is SocketException)
                {
                    socketErrorCode = ((SocketException) current).SocketErrorCode;
                }

                current = current.InnerException;
            } while (socketErrorCode == null && current != null);

            log(ex, socketErrorCode);
        }

        /// <summary>
        /// Construct a TCP _socket writer that writes to the given endPoint and _port.
        /// </summary>
        /// <param name="uri">Uri to open a TCP socket to.</param>
        /// <param name="maxQueueSize">The maximum number of log entries to queue before starting to drop entries.</param>
        public TcpSocketWriter(Uri uri, int maxQueueSize = 5000)
        {
            _eventQueue = new FixedSizeQueue<string>(maxQueueSize);
            _tokenSource = new CancellationTokenSource();

            Func<Uri, Task<Stream>> tryOpenSocket = async h =>
            {
                try
                {
                    TcpClient client = new TcpClient();
                    await client.ConnectAsync(uri.Host, uri.Port);
                    Stream stream = client.GetStream();
                    if (uri.Scheme.ToLower() != "tls")
                        return stream;

                    var sslStream = new SslStream(client.GetStream(), false, null, null);
                    await sslStream.AuthenticateAsClientAsync(uri.Host);
                    return sslStream;
                }
                catch (Exception e)
                {
                    LoggingFailureHandler(e);
                    throw;
                }
            };

            var threadReady = new TaskCompletionSource<bool>();

            Task queueListener = Task.Factory.StartNew(async () =>
            {
                try
                {
                    bool sslEnabled = uri.Scheme.ToLower() == "tls";
                    _stream = await _reconnectPolicy.ConnectAsync(tryOpenSocket, uri, _tokenSource.Token);
                    threadReady.SetResult(true); // Signal the calling thread that we are ready.

                    string entry = null;
                    while (_stream != null) // null indicates that the thread has been cancelled and cleaned up.
                    {
                        if (_tokenSource.Token.IsCancellationRequested)
                        {
                            _eventQueue.CompleteAdding();
                            // Post-condition: no further items will be added to the queue, so there will be a finite number of items to handle.
                            while (_eventQueue.Count > 0)
                            {
                                entry = _eventQueue.Dequeue();
                                try
                                {
                                    byte[] messsage = Encoding.UTF8.GetBytes(entry);
                                    await _stream.WriteAsync(messsage, 0, messsage.Length);
                                    await _stream.FlushAsync();
                                }
                                catch (SocketException ex)
                                {
                                    LoggingFailureHandler(ex);
                                }
                            }
                            break;
                        }
                        if (entry == null)
                        {
                            entry = _eventQueue.Dequeue(_tokenSource.Token);
                        }
                        else
                        {
                            try
                            {
                                byte[] messsage = Encoding.UTF8.GetBytes(entry);
                                await _stream.WriteAsync(messsage, 0, messsage.Length);
                                await _stream.FlushAsync();
                                // No exception, it was sent
                                entry = null;
                            }
                            catch (IOException ex)
                            {
                                LoggingFailureHandler(ex);
                                _stream = await _reconnectPolicy.ConnectAsync(tryOpenSocket, uri, _tokenSource.Token);
                            }
                            catch (SocketException ex)
                            {
                                LoggingFailureHandler(ex);
                                _stream = await _reconnectPolicy.ConnectAsync(tryOpenSocket, uri, _tokenSource.Token);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LoggingFailureHandler(e);
                }
                finally
                {
                    if (_stream != null)
                    {
                        _stream.Dispose();
                    }

                    _disposed.SetResult(true);
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            threadReady.Task.Wait(TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            // The following operations are idempotent. Issue a cancellation to tell the
            // writer thread to stop the queue from accepting entries and write what it has
            // before cleaning up, then wait until that cleanup is finished.
            _tokenSource.Cancel();
            Task.Run(async () => await _disposed.Task).Wait();
        }

        /// <summary>
        /// Push a string onto the queue to be written.
        /// </summary>
        /// <param name="entry">The string to be written to the TCP _socket.</param>
        public void Enqueue(string entry)
        {
            _eventQueue.Enqueue(entry);
        }
    }

    /// <summary>
    /// TcpConnectionPolicy implementation that tries to reconnect after
    /// increasingly long intervals.
    /// </summary>
    /// <remarks>
    /// The intervals double every time, starting from 0s, 1s, 2s, 4s, ...
    /// until 10 minutes between connections, when it plateaus and does
    /// not increase the interval length any further.
    /// </remarks>
    public class ExponentialBackoffTcpReconnectionPolicy
    {
        private readonly int ceiling = 10 * 60; // 10 minutes in seconds

        public async Task<Stream> ConnectAsync(Func<Uri, Task<Stream>> connect, Uri host, CancellationToken cancellationToken)
        {
            int delay = 1; // in seconds
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    //Log.Debug("Attempting to connect to TCP endpoint {host} after delay of {delay} seconds", host, delay);
                    return await connect(host);
                }
                catch (SocketException) { }

                // If this is cancelled via the cancellationToken instead of
                // completing its delay, the next while-loop test will fail,
                // the loop will terminate, and the method will return null
                // with no additional connection attempts.
                await Task.Delay(delay * 1000, cancellationToken);
                // The nth delay is min(10 minutes, 2^n - 1 seconds).
                delay = Math.Min((delay + 1) * 2 - 1, ceiling);
            }

            // cancellationToken has been cancelled.
            return null;
        }
    }

    /// <summary>
    /// A queue with a maximum size. When the queue is at its maximum size
    /// and a new item is queued, the oldest item in the queue is dropped.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class FixedSizeQueue<T>
    {
        private int Size { get; }
        private readonly IProgress<bool> _progress = new Progress<bool>();
        private bool IsCompleted { get; set; }

        private readonly BlockingCollection<T> _collection = new BlockingCollection<T>();

        public FixedSizeQueue(int size)
        {
            Size = size;
            IsCompleted = false;
        }

        public void Enqueue(T obj)
        {
            lock (this)
            {
                if (IsCompleted)
                {
                    throw new InvalidOperationException("Tried to add an item to a completed queue.");
                }

                _collection.Add(obj);

                while (_collection.Count > Size)
                {
                    _collection.Take();
                }
                _progress.Report(true);
            }
        }

        public void CompleteAdding()
        {
            lock (this)
            {
                IsCompleted = true;
            }
        }

        public T Dequeue(CancellationToken cancellationToken)
        {
            return _collection.Take(cancellationToken);
        }

        public T Dequeue()
        {
            return _collection.Take();
        }


        public decimal Count => _collection.Count;
    }
}