using System;
using System.IO;
using Blog.Core.Common.Https;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Common.Extensions;

public static class HttpResponseExceptions
{
    public static string GetResponseBody(this HttpResponse response)
    {
        if (response is null)
        {
            return string.Empty;
        }

        //原始HttpResponseStream 无法读取
        //实际上只是个包装类,内部使用了HttpResponsePipeWriter write
        switch (response.Body)
        {
            case FluentHttpResponseStream:
            case MemoryStream:
            {
                response.Body.Position = 0;
                using var stream = new StreamReader(response.Body, leaveOpen: true);
                var body = stream.ReadToEnd();
                response.Body.Position = 0;
                return body;
            }
            default:
                // throw new ApplicationException("The response body is not a FluentHttpResponseStream");
                return string.Empty;
        }
    }
}