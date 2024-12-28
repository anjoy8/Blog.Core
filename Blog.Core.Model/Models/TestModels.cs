namespace Blog.Core.Model.Models
{
    [MigrateVersion("1.0.0")]
    public class TestMuchTableResult
    {
        public string moduleName { get; set; }
        public string permName { get; set; }
        public long rid { get; set; }
        public long mid { get; set; }
        public long? pid { get; set; }

    }
}
