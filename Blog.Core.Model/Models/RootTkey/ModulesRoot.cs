using Newtonsoft.Json;
using SqlSugar;

namespace Blog.Core.Model
{
    /// <summary>
    /// 接口API地址信息表 
    /// 父类
    /// </summary>
    public class ModulesRoot<Tkey> : RootEntityTkey<int>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Tkey ParentId { get; set; }
       
    }
}
