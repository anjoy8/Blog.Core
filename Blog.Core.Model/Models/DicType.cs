
namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 字典类型表(单数据)
    /// </summary>
    public class DicType: RootEntityTkey<long>
    {
        /// <summary>
        /// 字典code
        /// </summary>
        public string code { set; get; }
        /// <summary>
        /// 字典名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 字典内容
        /// </summary>
        public string content { set; get; }
        /// <summary>
        /// 字典内容2
        /// </summary>
        public string content2 { set; get; }
        /// <summary>
        /// 字典内容3
        /// </summary>
        public string content3 { set; get; }
        /// <summary>
        /// 字典描述
        /// </summary>
        public string description { set; get; }
    }
}
