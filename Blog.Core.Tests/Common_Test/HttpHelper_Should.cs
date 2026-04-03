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
           var handler = new HttpClientHandler
           {
               ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
           };
           
           using var client = new HttpClient(handler);
        }

    }
}
