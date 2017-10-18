using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TDF.Demo.Service.Dtos.SystemManage;

namespace TDF.Demo.AdminWeb.Areas.System.Models.Members
{
    public class PassWordModel : PassWordDto, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //解密
            PwdDecrypt();

            if (string.IsNullOrEmpty(OldPassword))
            {
                yield return new ValidationResult("旧密码不能为空");
            }

            if (string.IsNullOrEmpty(Password))
            {
                yield return new ValidationResult("密码不能为空");
            }
            if (Password != ComfirmPassword)
            {
                yield return new ValidationResult("您两次输入的密码不一致");
            }
            if (Password == OldPassword)
            {
                yield return new ValidationResult("新密码不能和原来的密码一样");
            }
            if (!CheckedPasswordRegular(Password))
            {
                yield return new ValidationResult("密码由6-12位必须包含数字、字母（支持特殊符号）的组合");
            }
        }

        /// <summary>
        /// 密码解密
        /// </summary>
        private void PwdDecrypt()
        {
            //ComfirmPassword = RSAEncryptHelper.Decrypt(CollegeConfigs.PasswordPrivateKey, ComfirmPassword);
            //Password = RSAEncryptHelper.Decrypt(CollegeConfigs.PasswordPrivateKey, Password); //将数据包中的密码解密
            //OldPassword = RSAEncryptHelper.Decrypt(CollegeConfigs.PasswordPrivateKey, OldPassword);//将数据包中的密码解密
        }

        /// <summary>
        /// 验证密码规则
        /// 规则：密码由8-12位必须包含数字、字母（支持特殊符号）的组合
        /// </summary>
        /// <param name="password"></param>
        private bool CheckedPasswordRegular(string password)
        {
            //密码由8-12位必须包含数字、字母（支持特殊符号）的组合
            Regex sr = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z])[\da-zA-Z~!@#$%^&*]{6,12}$");
            return sr.IsMatch(password);
        }
    }
}