using FluentValidation;
using System.Text.RegularExpressions;

namespace Blog.Core.Filter
{
    public class UserRegisterVo
    {
        public string WxUid { get; set; }

        public string Telphone { get; set; }

        public string NickName { get; set; }

        public string SourceType { get; set; }

    }

    public class UserRegisterVoValidator : AbstractValidator<UserRegisterVo>
    {
        public UserRegisterVoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.NickName) || !string.IsNullOrEmpty(x.Telphone), () =>
            {
                RuleFor(x => x.NickName).Must(e => IsLegalName(e)).WithMessage("请填写合法的姓名，必须是汉字和字母");
                RuleFor(x => x.Telphone).Must(e => IsLegalPhone(e)).WithMessage("请填写正确的手机号码");
            });

        }

        public static bool IsLegalName(string username)
        {
            //判断用户名是否合法
            const string pattern = "(^([A-Za-z]|[\u4E00-\u9FA5]){1,10}$)";
            return (!string.IsNullOrEmpty(username) && Regex.IsMatch(username, pattern));
        }
        public static bool IsLegalPhone(string phone)
        {
            //判断手机号
            const string pattern = "(^1\\d{10}$)";
            return (!string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, pattern));
        }
    }

}