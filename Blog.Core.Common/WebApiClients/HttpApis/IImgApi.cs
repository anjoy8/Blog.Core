using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.DataAnnotations;
using WebApiClient.Parameterables;
namespace Blog.Core.Common.WebApiClients
{
    /// <summary>
    /// 图片管理
    /// </summary>
    [TraceFilter]
    public interface IImgApi : IHttpApi
    {
        /// <summary>
        /// (Auth)
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPost("api/Img")]
        ITask<HttpResponseMessage> ImgAsync([JsonContent] object body);

        /// <summary>
        /// (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Success</returns>
        [HttpPut("api/Img/{id}")]
        ITask<HttpResponseMessage> Img2Async([Required] int id, [JsonContent] string body);

        /// <summary>
        /// (Auth)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success</returns>
        [HttpDelete("api/Img/{id}")]
        ITask<HttpResponseMessage> Img3Async([Required] int id);

        /// <summary>
        /// 下载图片（支持中文字符） (Auth)
        /// </summary>
        /// <returns>Success</returns>
        [HttpGet("images/Down/Pic")]
        ITask<HttpResponseMessage> PicAsync();

        /// <summary>
        /// 上传图片,多文件，可以使用 postman 测试，
        /// 如果是单文件，可以 参数写 IFormFile file1 (Auth)
        /// </summary>
        /// <returns>Success</returns>
        [HttpPost("images/Upload/Pic")]
        ITask<StringMessageModel> Pic2Async();

    }
}