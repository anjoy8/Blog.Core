using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System;

namespace Blog.Core.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// </summary>
    public class BlogUserAuditAOP : CacheAOPbase
    {
        private readonly IHttpContextAccessor _accessor;

        public BlogUserAuditAOP(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public override void Intercept(IInvocation invocation)
        {
            string UserName = _accessor.HttpContext?.User?.Identity?.Name;

            //对当前方法的特性验证
            if (invocation.Method.Name?.ToLower() == "add" || invocation.Method.Name?.ToLower() == "update")
            {

                if (invocation.Arguments.Length == 1)
                {
                    if (invocation.Arguments[0].GetType().IsClass)
                    {
                        dynamic argModel = invocation.Arguments[0];
                        var getType = argModel.GetType();
                        if (invocation.Method.Name?.ToLower() == "add")
                        {
                            if (getType.GetProperty("CreateBy") != null)
                            {
                                argModel.CreateBy = UserName; 
                            }
                            if (getType.GetProperty("bCreateTime") != null)
                            {
                                argModel.bCreateTime = DateTime.Now; 
                            }
                        }
                        if (getType.GetProperty("bUpdateTime") != null)
                        {
                            argModel.bUpdateTime = DateTime.Now; 
                        }
                        if (getType.GetProperty("ModifyBy") != null)
                        {
                            argModel.ModifyBy = UserName; 
                        }
                        if (getType.GetProperty("bsubmitter") != null)
                        {
                            argModel.bsubmitter = UserName; 
                        }

                        invocation.Arguments[0] = argModel;
                    }
                }
                invocation.Proceed();
            }
            else
            {
                invocation.Proceed();
            }
        }
    }

}
