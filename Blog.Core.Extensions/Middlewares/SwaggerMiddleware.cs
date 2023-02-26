using System;
using System.IO;
using System.Linq;
using Blog.Core.Common;
using log4net;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using static Blog.Core.Extensions.CustomApiVersion;

namespace Blog.Core.Extensions.Middlewares
{
    /// <summary>
    /// Swagger中间件
    /// </summary>
    public static class SwaggerMiddleware
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SwaggerMiddleware));
        public static void UseSwaggerMiddle(this IApplicationBuilder app, Func<Stream> streamHtml)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                var apiName = AppSettings.app(new string[] { "Startup", "ApiName" });
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{apiName} {version}");
                });

                c.SwaggerEndpoint($"https://petstore.swagger.io/v2/swagger.json", $"{apiName} pet");

                // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：{项目名.index.html}
                if (streamHtml.Invoke() == null)
                {
                    var msg = "index.html的属性，必须设置为嵌入的资源";
                    Log.Error(msg);
                    throw new Exception(msg);
                }
                c.IndexStream = streamHtml;
                c.DocExpansion(DocExpansion.None); //->修改界面打开时自动折叠

                if (Permissions.IsUseIds4)
                {
                    c.OAuthClientId("blogadminjs"); 
                }


                // 路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });
        }
    }
}
