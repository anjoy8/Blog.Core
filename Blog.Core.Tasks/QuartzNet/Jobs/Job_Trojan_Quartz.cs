 
using Blog.Core.IServices;
using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;
using Blog.Core.Repository.UnitOfWorks;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到(任务类要去JobSetup添加注入)
/// </summary>
namespace Blog.Core.Tasks
{
    public class Job_Trojan_Quartz : JobBase, IJob
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        public IBaseServices<TrojanDetails>_DetailServices;
        private readonly ITrojanUsersServices _TrojanUsers; 
        private readonly ILogger<Job_Trojan_Quartz> _logger;

        public Job_Trojan_Quartz(IUnitOfWorkManage unitOfWorkManage, IBaseServices<TrojanDetails> iusers_DetailServices, ITrojanUsersServices trojanUsers, ILogger<Job_Trojan_Quartz> logger, ITasksQzServices tasksQzServices, ITasksLogServices tasksLogServices)
            : base(tasksQzServices, tasksLogServices)
        {
            _tasksQzServices = tasksQzServices;
            _unitOfWorkManage = unitOfWorkManage;
            _DetailServices = iusers_DetailServices;
            _TrojanUsers = trojanUsers; 
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        { 
            //var param = context.MergedJobDataMap;
            // 可以直接获取 JobDetail 的值
            var jobKey = context.JobDetail.Key;
            var jobId = jobKey.Name; 
            var executeLog = await ExecuteJob(context, async () => await Run(context, jobId.ObjToInt()));

        }
        public async Task Run(IJobExecutionContext context, int jobid)
        { 
            if (jobid > 0)
            {
                try
                {
                    //获取每月用户的数据
                    _unitOfWorkManage.BeginTran();
                    var now = DateTime.Now.AddMonths(-1);

                    var list = await _TrojanUsers.Query();
                    List<TrojanDetails> ls = new List<TrojanDetails>();
                    foreach (var us in list)
                    {
                        TrojanDetails u = new TrojanDetails();
                        u.calDate = now;
                        u.userId = us.id;
                        u.download = us.download;
                        u.upload = us.upload;
                        //清零
                        us.download = 0;
                        us.upload = 0;
                        ls.Add(u);
                    }
                    await _TrojanUsers.Update(list);
                    await _DetailServices.Add(ls);
                    _unitOfWorkManage.CommitTran();
                }
                catch (Exception)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw;
                }
            }  
        }
    }



}
