using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>任务计划表</summary>
    public class TasksQz
    {
        /// <summary>任务名称</summary>
        [AliasAs("name")]
        public string Name { get; set; }

        /// <summary>任务分组</summary>
        [AliasAs("jobGroup")]
        public string JobGroup { get; set; }

        /// <summary>任务运行时间表达式</summary>
        [AliasAs("cron")]
        public string Cron { get; set; }

        /// <summary>任务所在DLL对应的程序集名称</summary>
        [AliasAs("assemblyName")]
        public string AssemblyName { get; set; }

        /// <summary>任务所在类</summary>
        [AliasAs("className")]
        public string ClassName { get; set; }

        /// <summary>任务描述</summary>
        [AliasAs("remark")]
        public string Remark { get; set; }

        /// <summary>执行次数</summary>
        [AliasAs("runTimes")]
        public int RunTimes { get; set; }

        /// <summary>开始时间</summary>
        [AliasAs("beginTime")]
        public System.DateTimeOffset? BeginTime { get; set; }

        /// <summary>结束时间</summary>
        [AliasAs("endTime")]
        public System.DateTimeOffset? EndTime { get; set; }

        /// <summary>触发器类型（0、simple 1、cron）</summary>
        [AliasAs("triggerType")]
        public int TriggerType { get; set; }

        /// <summary>执行间隔时间, 秒为单位</summary>
        [AliasAs("intervalSecond")]
        public int IntervalSecond { get; set; }

        /// <summary>是否启动</summary>
        [AliasAs("isStart")]
        public bool IsStart { get; set; }

        /// <summary>执行传参</summary>
        [AliasAs("jobParams")]
        public string JobParams { get; set; }

        [AliasAs("isDeleted")]
        public bool? IsDeleted { get; set; }

        /// <summary>创建时间</summary>
        [AliasAs("createTime")]
        public System.DateTimeOffset CreateTime { get; set; }

        /// <summary>ID</summary>
        [AliasAs("id")]
        public int Id { get; set; }

    }
}