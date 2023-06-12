using System;
using Blog.Core.Common;
using Castle.DynamicProxy;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common.Caches;

namespace Blog.Core.AOP
{
	/// <summary>
	/// 面向切面的缓存使用
	/// </summary>
	public class BlogCacheAOP : CacheAOPbase
	{
		//通过注入的方式，把缓存操作接口通过构造函数注入
		private readonly ICaching _cache;

		public BlogCacheAOP(ICaching cache)
		{
			_cache = cache;
		}

		//Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
		public override void Intercept(IInvocation invocation)
		{
			var method = invocation.MethodInvocationTarget ?? invocation.Method;
			//对当前方法的特性验证
			//如果需要验证
			var CachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute));
			if (CachingAttribute is CachingAttribute qCachingAttribute)
			{
				//获取自定义缓存键
				var cacheKey = CustomCacheKey(invocation);
				if (_cache.Exists(cacheKey))
				{
					//将当前获取到的缓存值，赋值给当前执行方法
					Type returnType;
					if (typeof(Task).IsAssignableFrom(method.ReturnType))
					{
						returnType = method.ReturnType.GenericTypeArguments.FirstOrDefault();
					}
					else
					{
						returnType = method.ReturnType;
					}

					//根据key获取相应的缓存值
					dynamic cacheValue = _cache.Get(returnType, cacheKey);
					invocation.ReturnValue = (typeof(Task).IsAssignableFrom(method.ReturnType)) ? Task.FromResult(cacheValue) : cacheValue;
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
						response = resultProperty?.GetValue(invocation.ReturnValue);
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
				invocation.Proceed(); //直接执行被拦截方法
			}
		}
	}
}