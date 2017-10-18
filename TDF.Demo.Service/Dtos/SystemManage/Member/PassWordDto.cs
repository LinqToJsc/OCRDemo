using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public class PassWordDto : IDto
    {
        public Guid? Id { get; set; }

        public string OldPassword { get; set; }

        //[RegularExpression(@"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,30}$", ErrorMessage = "密码必须是数字和字母的组合,最少8位")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z])[\da-zA-Z~!@#$%^&*]{8,12}$", ErrorMessage = "密码由8-12位必须包含数字、字母（支持特殊符号）的组合")]
        [Required(ErrorMessage = "密码码不能为空")]
        public string Password { get; set; }

        [Required(ErrorMessage = "密码码不能为空")]
        public string ComfirmPassword { get; set; }
    }
}
