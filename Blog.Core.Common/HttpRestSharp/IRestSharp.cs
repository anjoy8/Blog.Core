using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Common.HttpRestSharp
{
    /// <summary>
    /// API请求执行者接口
    /// </summary>
    public interface IRestSharp
    {
        /// <summary>
        /// 同步执行方法
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IRestResponse Execute(IRestRequest request);

        /// <summary>
        /// 同步执行方法
        /// </summary>
        /// <typeparam name="T">返回值</typeparam>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        T Execute<T>(IRestRequest request) where T : new();

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse> callback);

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>> callback) where T : new();
    }
}
