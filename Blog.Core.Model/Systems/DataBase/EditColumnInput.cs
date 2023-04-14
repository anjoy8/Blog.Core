namespace Blog.Core.Model.Systems.DataBase;

public class EditColumnInput
{
    public string ConfigId { get; set; }
    public string TableName { get; set; }
    public string DbColumnName { get; set; }
    public string ColumnDescription { get; set; }
}