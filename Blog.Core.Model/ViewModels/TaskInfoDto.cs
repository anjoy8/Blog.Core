namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 调度任务触发器信息实体
    /// </summary>
    public class TaskInfoDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string jobId { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string jobName { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        public string jobGroup { get; set; }
        /// <summary>
        /// 触发器ID
        /// </summary>
        public string triggerId { get; set; }
        /// <summary>
        /// 触发器名称
        /// </summary>
        public string triggerName { get; set; }
        /// <summary>
        /// 触发器分组
        /// </summary>
        public string triggerGroup { get; set; }
        /// <summary>
        /// 触发器状态
        /// </summary>
        public string triggerStatus { get; set; }
    }
}
