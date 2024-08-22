using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Blog.Core.Tasks
{
    public class JobBase
    {
        public ITasksQzServices _tasksQzServices;
        public ITasksLogServices _tasksLogServices;
        public JobBase(ITasksQzServices tasksQzServices, ITasksLogServices tasksLogServices)
        {
            _tasksQzServices = tasksQzServices;
            _tasksLogServices = tasksLogServices;
        }
        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public async Task<string> ExecuteJob(IJobExecutionContext context, Func<Task> func)
        {
            //记录Job
            TasksLog tasksLog = new TasksLog();
            //JOBID
            int jobid = context.JobDetail.Key.Name.ObjToInt();
            //JOB组名
            string groupName = context.JobDetail.Key.Group;
            //日志
            tasksLog.JobId = jobid;
            tasksLog.RunTime = DateTime.Now;
            string jobHistory = $"【{tasksLog.RunTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行开始】【Id：{jobid}，组别：{groupName}】";
            try
            {
                await func();//执行任务
                tasksLog.EndTime = DateTime.Now;
                tasksLog.RunResult = true;
                jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行成功】";

                JobDataMap jobPars = context.JobDetail.JobDataMap;
                tasksLog.RunPars = jobPars.GetString("JobParam");
            }
            catch (Exception ex)
            {
                tasksLog.EndTime = DateTime.Now;
                tasksLog.RunResult = false;
                //JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                //e2.RefireImmediately = true;
                tasksLog.ErrMessage = ex.Message;
                tasksLog.ErrStackTrace = ex.StackTrace;
                jobHistory += $"，【{tasksLog.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}】【执行失败:{ex.Message}】";
            }
            finally
            {
                tasksLog.TotalTime = Math.Round((tasksLog.EndTime - tasksLog.RunTime).TotalSeconds,3);
                jobHistory += $"(耗时:{tasksLog.TotalTime}秒)";
                if (_tasksQzServices != null)
                {
                    var model = await _tasksQzServices.QueryById(jobid);
                    if (model != null)
                    {
                        if(_tasksLogServices != null) await _tasksLogServices.Add(tasksLog);
                        model.RunTimes += 1;
                        if (model.TriggerType == 0) model.CycleHasRunTimes += 1;
                        if (model.TriggerType == 0 && model.CycleRunTimes != 0 && model.CycleHasRunTimes >= model.CycleRunTimes) model.IsStart = false;//循环完善,当循环任务完成后,停止该任务,防止下次启动再次执行
                        var separator = "<br>";
                        // 这里注意数据库字段的长度问题，超过限制，会造成数据库remark不更新问题。
                        model.Remark =
                            $"{jobHistory}{separator}" + string.Join(separator, StringHelper.GetTopDataBySeparator(model.Remark, separator, 9));
                        await _tasksQzServices.Update(model);
                    }
                }
            }

            //Console.Out.WriteLine(jobHistory);
            return jobHistory;
        }
    }

}
