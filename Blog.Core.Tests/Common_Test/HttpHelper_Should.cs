using Blog.Core.Common.Helper;
using Xunit;

namespace Blog.Core.Tests.Common_Test
{
    public class HttpHelper_Should
    {

        [Fact]
        public void Get_Async_Test()
        {
            var responseString = HttpHelper.GetAsync("http://apk.neters.club/api/Blog").Result;

            Assert.NotNull(responseString);
        }

        [Fact]
        public void Post_Async_Test()
        {
            var responseString = HttpHelper.PostAsync("http://apk.neters.club/api/Login/swgLogin", "{\"name\":\"admin\",\"pwd\":\"admin\"}").Result;

            Assert.NotNull(responseString);
        }

    }
}
