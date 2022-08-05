using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using Blog.Core.Common.Extensions;
using Blog.Core.IRepository.UnitOfWork;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Blog.Core.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;

        private int _tranCount { get; set; }
        public int TranCount => _tranCount;
        public readonly ConcurrentStack<string> TranStack = new();

        public UnitOfWork(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWork> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            _tranCount = 0;
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetDbClient()
        {
            // 必须要as，后边会用到切换数据库操作
            return _sqlSugarClient as SqlSugarScope;
        }


        public void BeginTran()
        {
            lock (this)
            {
                _tranCount++;
                GetDbClient().BeginTran();
            }
        }

        public void BeginTran(MethodInfo method)
        {
            lock (this)
            {
                GetDbClient().BeginTran();
                TranStack.Push(method.GetFullName());
                _tranCount = TranStack.Count;
            }
        }

        public void CommitTran()
        {
            lock (this)
            {
                _tranCount--;
                if (_tranCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                    }
                }
            }
        }

        public void CommitTran(MethodInfo method)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }


                if (result == method.GetFullName())
                {
                    try
                    {
                        GetDbClient().CommitTran();

                        _logger.LogDebug($"Commit Transaction");
                        Console.WriteLine($"Commit Transaction");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                        _logger.LogDebug($"Commit Error , Rollback Transaction");
                    }
                    finally
                    {
                        while (!TranStack.TryPop(out _))
                        {
                            Thread.Sleep(1);
                        }

                        _tranCount = TranStack.Count;
                    }
                }
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                _tranCount--;
                GetDbClient().RollbackTran();
            }
        }

        public void RollbackTran(MethodInfo method)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }

                if (result == method.GetFullName())
                {
                    GetDbClient().RollbackTran();
                    _logger.LogDebug($"Rollback Transaction");
                    Console.WriteLine($"Rollback Transaction");
                    while (!TranStack.TryPop(out _))
                    {
                        Thread.Sleep(1);
                    }

                    _tranCount = TranStack.Count;
                }
            }
        }
    }
}