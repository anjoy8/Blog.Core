namespace Blog.Core.Model.ViewModels
{
    /// <summary>
    /// 服务器VM
    /// </summary>
    public class ServerViewModel
    {
        /// <summary>
        /// 环境变量
        /// </summary>
        public string EnvironmentName { get; set; }
        /// <summary>
        /// 系统架构
        /// </summary>
        public string OSArchitecture { get; set; }
        /// <summary>
        /// ContentRootPath
        /// </summary>
        public string ContentRootPath { get; set; }
        /// <summary>
        /// WebRootPath
        /// </summary>
        public string WebRootPath { get; set; }
        /// <summary>
        /// .NET Core版本
        /// </summary>
        public string FrameworkDescription { get; set; }
        /// <summary>
        /// 内存占用
        /// </summary>
        public string MemoryFootprint { get; set; }
        /// <summary>
        /// 启动时间
        /// </summary>
        public string WorkingTime { get; set; }


    }
}
