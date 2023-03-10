using SqlSugar;
using System;

namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 任务日志表
    /// </summary>
    public class TasksLog : RootEntityTkey<int>
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// 任务耗时
        /// </summary>
        public double TotalTime { get; set; }
        /// <summary>
        /// 执行结果(0-失败 1-成功)
        /// </summary>
        public bool RunResult { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public DateTime RunTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 执行参数
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string RunPars { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string ErrMessage { get; set; }
        /// <summary>
        /// 异常堆栈
        /// </summary>
        [SugarColumn(Length = 2000, IsNullable = true)]
        public string ErrStackTrace { get; set; }
        /// <summary>
        /// 创建ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string JobGroup { get; set; }
    }
}
