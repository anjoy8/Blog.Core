namespace Blog.Core.Model.Base.RootTkey.Interface;

/// <summary>
/// 软删除 过滤器
/// </summary>
public interface IDeleteFilter
{
    public bool IsDeleted { get; set; }
}