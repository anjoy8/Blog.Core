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
			return default;
		}

		if (response.Body is FluentHttpResponseStream responseBody)
		{
			response.Body.Position = 0;
			//不关闭底层流
			using StreamReader stream = new StreamReader(responseBody, leaveOpen: true);
			string body = stream.ReadToEnd();
			response.Body.Position = 0;
			return body;
		}
		else
		{
			//原始HttpResponseStream 无法读取
			//实际上只是个包装类,内部使用了HttpResponsePipeWriter write
			throw new ApplicationException("The response body is not a FluentHttpResponseStream");
		}
		
		return default;
	}
}