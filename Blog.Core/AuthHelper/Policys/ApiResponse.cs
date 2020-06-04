using Blog.Core.Model;

namespace Blog.Core.AuthHelper.Policys
{
    public class ApiResponse
    {
        public int Status { get; set; } = 404;
        public string Value { get; set; } = "No Found";
        public MessageModel<string> MessageModel = new MessageModel<string>() { };

        public ApiResponse(StatusCode apiCode, string msg = null)
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
                case StatusCode.CODE500:
                    {
                        Status = 500;
                        Value = msg;
                    }
                    break;
            }

            MessageModel = new MessageModel<string>()
            {
                status = Status,
                msg = Value,
                success = false
            };
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
