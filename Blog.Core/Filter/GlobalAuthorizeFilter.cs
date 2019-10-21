using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Filter
{
    /// <summary>
    /// Summary:全局路由权限公约
    /// Remarks:目的是针对不同的路由，采用不同的授权过滤器
    /// 如果 controller 上不加 [Authorize] 特性，默认都是 Permission 策略
    /// 否则，如果想特例其他授权机制的话，需要在 controller 上带上  [Authorize]，然后再action上自定义授权即可，比如 [Authorize(Roles = "Admin")]
    /// </summary>
    public class GlobalRouteAuthorizeConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var c in application.Controllers)
            {
                if (!c.Filters.Any(e => e is AuthorizeFilter))
                {
                    // 没有写特性，就用全局的 Permission 授权
                    c.Filters.Add(new AuthorizeFilter(Permissions.Name));
                }
                else {
                    // 写了特性，[Authorize] 或 [AllowAnonymous] ，根据情况进行权限认证
                }

            }
        }
    }

    /// <summary>
    /// 全局权限过滤器【无效】
    /// </summary>
    public class GlobalAuthorizeFilter : AuthorizeFilter
    {

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAsyncAuthorizationFilter && item != this))
            {
                return Task.FromResult(0);
            }


            return base.OnAuthorizationAsync(context);

          
        }
    }



}
