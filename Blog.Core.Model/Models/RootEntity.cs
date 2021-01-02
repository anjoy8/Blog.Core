using SqlSugar;

namespace Blog.Core.Model
{
    public class RootEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public int Id { get; set; }

      
    }
}