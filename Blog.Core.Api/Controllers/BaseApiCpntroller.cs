using Blog.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog.Core.Controllers
{
    public class BaseApiCpntroller : Controller
    {

        public MessageModel<T> Success<T>(T data, string msg = "成功")
        {
            return new MessageModel<T>()
            {
                success = true,
                msg = msg,
                response = data,
            };
        }

        public MessageModel Success(string msg = "成功")
        {
            return new MessageModel()
            {
                success = true,
                msg = msg,
                response = null,
            };
        }

        public MessageModel<string> Failed(string msg = "失败", int status = 500)
        {
            return new MessageModel<string>()
            {
                success = false,
                status = status,
                msg = msg,
                response = null,
            };
        }

        public MessageModel<T> Failed<T>(string msg = "失败", int status = 500)
        {
            return new MessageModel<T>()
            {
                success = false,
                status = status,
                msg = msg,
                response = default,
            };
        }

        public MessageModel<PageModel<T>> SuccessPage<T>(int page, int dataCount, List<T> data, int pageCount, string msg = "获取成功")
        {

            return new MessageModel<PageModel<T>>()
            {
                success = true,
                msg = msg,
                response = new PageModel<T>()
                {
                    page = page,
                    dataCount = dataCount,
                    data = data,
                    pageCount = pageCount,
                }
            };
        }

        public MessageModel<PageModel<T>> SuccessPage<T>(PageModel<T> pageModel, string msg = "获取成功")
        {

            return new MessageModel<PageModel<T>>()
            {
                success = true,
                msg = msg,
                response = new PageModel<T>()
                {
                    page = pageModel.page,
                    dataCount = pageModel.dataCount,
                    data = pageModel.data,
                    pageCount = pageModel.pageCount,
                }
            };
        }
    }
}
