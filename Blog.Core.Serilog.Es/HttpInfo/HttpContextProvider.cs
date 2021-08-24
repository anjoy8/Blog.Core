using Microsoft.AspNetCore.Http;

namespace Blog.Core.Serilog.Es.HttpInfo
{
    public static class HttpContextProvider
    {
        private static IHttpContextAccessor _accessor;

        public static HttpContext GetCurrent()
        {
            var context = _accessor?.HttpContext;
            return context;
        }
        public static void ConfigureAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }

}
