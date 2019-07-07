using Blog.Core.Common;
using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// </summary>
    public class BlogRedisCacheAOP : CacheAOPbase
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly IRedisCacheManager _cache;
        public BlogRedisCacheAOP(IRedisCacheManager cache)
        {
            _cache = cache;
        }
        
        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;

            if (qCachingAttribute != null)
            {
                //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                //注意是 string 类型，方法GetValue
                var cacheValue = _cache.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    var type = invocation.Method.ReturnType;
                    var resultTypes = type.GenericTypeArguments;
                    if (type.FullName == "System.Void")
                    {
                        return;
                    }
                    object response;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        //返回Task<T>
                        if (resultTypes.Any())
                        {
                            var resultType = resultTypes.FirstOrDefault();
                            // 核心1，直接获取 dynamic 类型
                            dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, resultType);
                            //dynamic temp = System.Convert.ChangeType(cacheValue, resultType);
                            // System.Convert.ChangeType(Task.FromResult(temp), type);
                            response = Task.FromResult(temp);

                        }
                        else
                        {
                            //Task 无返回方法 指定时间内不允许重新运行
                            response = Task.Yield();
                        }
                    }
                    else
                    {
                        // 核心2，要进行 ChangeType
                        response = Convert.ChangeType(_cache.Get<object>(cacheKey), type);
                    }

                    invocation.ReturnValue = response;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();

                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;

                    //Type type = invocation.ReturnValue?.GetType();
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    if (response == null) response = string.Empty;

                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(qCachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }
    }

}
