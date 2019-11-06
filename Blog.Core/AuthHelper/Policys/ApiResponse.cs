using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper.Policys
{
    public class ApiResponse
    {
        public int Status { get; set; } = 404;
        public object Value { get; set; } = "No Found";

        public ApiResponse(StatusCode apiCode)
        {
            switch (apiCode)
            {
                case StatusCode.CODE401:
                    {
                        Status = 401;
                        Value = "很抱歉，您无权访问该接口，请确保已经登录!";
                    }
                    break;
                case StatusCode.CODE403:
                    {
                        Status = 403;
                        Value = "很抱歉，您的访问权限等级不够，联系管理员!";
                    }
                    break;
            }
        }
    }

    public enum StatusCode
    {
        CODE401,
        CODE403,
        CODE404,
        CODE500
    }

}
