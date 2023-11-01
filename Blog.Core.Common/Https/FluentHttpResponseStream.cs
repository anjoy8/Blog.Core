using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace Blog.Core.Common.Https;

/// <summary>
/// 扩展 HttpResponseStream <br/>
/// 原始[HttpResponseStream]实际上只是个包装类,内部包装了[HttpResponsePipeWriter]来进行写入响应数据
/// </summary>
public class FluentHttpResponseStream : Stream
{
	private readonly IHttpBodyControlFeature _bodyControl;
	private readonly IHttpResponseBodyFeature _pipeWriter;
	private readonly MemoryStream _stream = new();

	public FluentHttpResponseStream(IHttpResponseBodyFeature pipeWriter, IHttpBodyControlFeature bodyControl)
	{
		_pipeWriter = pipeWriter;
		_bodyControl = bodyControl;
	}

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => _stream.CanSeek;

	public override bool CanWrite => _stream.CanWrite;

	public override long Length => _stream.Length;

	public override long Position { get => _stream.Position; set => _stream.Position = value; }

	public override void Flush()
	{
		if (!_bodyControl.AllowSynchronousIO)
		{
			throw new InvalidOperationException("SynchronousWritesDisallowed ");
		}
		_stream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return _stream.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return _stream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		_stream.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		WriteAsync(buffer, offset, count, default).GetAwaiter().GetResult();
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		_stream.Write(buffer, offset, count);
		return _pipeWriter.Writer.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
	}

	protected override void Dispose(bool disposing)
	{
		_stream.Dispose();
		base.Dispose(disposing);
	}
}