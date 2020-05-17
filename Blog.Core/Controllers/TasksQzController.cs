using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TasksQzController : ControllerBase
    {
        private readonly ITasksQzServices _tasksQzServices;
        private readonly ISchedulerCenter _schedulerCenter;

        public TasksQzController(ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter) 
        {
            _tasksQzServices = tasksQzServices;
            _schedulerCenter = schedulerCenter;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/Buttons/5
        [HttpGet]
        public async Task<MessageModel<PageModel<TasksQz>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;

            Expression<Func<TasksQz, bool>> whereExpression = a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key));

            var data = await _tasksQzServices.QueryPage(whereExpression, page, intPageSize, " Id desc ");

            return new MessageModel<PageModel<TasksQz>>()
            {
                msg = "获取成功",
                success = data.dataCount >= 0,
                response = data
            };

        }

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<string>> Post([FromBody] TasksQz tasksQz)
        {
            var data = new MessageModel<string>();

            var id = (await _tasksQzServices.Add(tasksQz));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }


        /// <summary>
        /// 修改计划任务
        /// </summary>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] TasksQz tasksQz)
        {
            var data = new MessageModel<string>();
            if (tasksQz != null && tasksQz.Id > 0)
            {
                data.success = await _tasksQzServices.Update(tasksQz);
                if (data.success)
                {
                    data.msg = "更新成功";
                    data.response = tasksQz?.Id.ObjToString();
                }
            }

            return data;
        }

        /// <summary>
        /// 启动计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> StartJob(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzServices.QueryById(jobId);
            var ResuleModel = await _schedulerCenter.AddScheduleJobAsync(model);
            if (ResuleModel.success)
            {
                model.IsStart = true;
                data.success = await _tasksQzServices.Update(model);
            }
            if (data.success)
            {
                data.msg = "启动成功";
                data.response = jobId.ObjToString();
            }
            return data;

        }
        /// <summary>
        /// 停止一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>        
        [HttpGet]
        public async Task<MessageModel<string>> StopJob(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzServices.QueryById(jobId);
            var ResuleModel = await _schedulerCenter.StopScheduleJobAsync(model);
            if (ResuleModel.success)
            {
                model.IsStart = false;
                data.success = await _tasksQzServices.Update(model);
            }
            if (data.success)
            {
                data.msg = "暂停成功";
                data.response = jobId.ObjToString();
            }
            return data;

        }
        /// <summary>
        /// 重启一个计划任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> ReCovery(int jobId)
        {
            var data = new MessageModel<string>();

            var model = await _tasksQzServices.QueryById(jobId);
            var ResuleModel = await _schedulerCenter.ResumeJob(model);
            if (ResuleModel.success)
            {
                model.IsStart = true;
                data.success = await _tasksQzServices.Update(model);
            }
            if (data.success)
            {
                data.msg = "重启成功";
                data.response = jobId.ObjToString();
            }
            return data;

        }
    }
}
