using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;
using TDF.Core.Caching;
using TDF.Core.Configuration;
using TDF.Core.Exceptions;
using TDF.Core.Tools;
using TDF.Web.Infrastructure;

namespace TDF.Web.Authentication.Models
{
    public class LoginModel : IValidatableObject
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        
        public string Code { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Configs.Instance.EnvironmentType == EnvironmentType.Dev)
            {
                yield break; 
            }
            if (!Configs.Instance.EnabledVerifyCode)
            {
                yield break;
            }
            if (string.IsNullOrEmpty(Code))
            {
                throw new KnownException("请输入验证码");
            }
            var verifykey = WebHelper.GetCookie(Configs.Instance.Verifykey);
            var rightCode = CacheManager.Get<string>(verifykey);
            //验证码用过一次就销毁[不论验证是否成功]
            CacheManager.Remove(verifykey);
            if (rightCode.IsEmpty() || MD5Helper.GetMD5(Code.ToLower(), 16) != rightCode)
            {
                yield return new ValidationResult("验证码错误，请重新输入");
            }
        }
    }
}
