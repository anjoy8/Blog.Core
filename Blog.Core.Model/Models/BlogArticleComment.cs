namespace Blog.Core.Model.Models;

/// <summary>
/// 博客文章 评论
/// </summary>
[MigrateVersion("1.0.0")]
public class BlogArticleComment : RootEntityTkey<long>
{
    public long bID { get; set; }

    public string Comment { get; set; }


    public string UserId { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public SysUserInfo User { get; set; }
}