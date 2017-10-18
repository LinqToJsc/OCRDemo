using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.DataAnnotations;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public class SystemMemberDto : IDto
    {
        public Guid? Id { get; set; }

        [Required, MaxLength(32)]
        public string Account { get; set; }

        /// <summary>
        /// true=男，false=女
        /// </summary>
        public bool? Gender { get; set; }

        [TDF.Core.DataAnnotations.Phone(Required = false)]
        public string MobilePhone { get; set; }

        [Email(Required = false)]
        public string Email { get; set; }


        public bool? EnabledMark { get; set; }

        public bool IsSuperAdmin { get; set; }

        [Required, MaxLength(100)]
        public string RealName { get; set; }

        public string QQ { get; set; }

        [MaxLength(256)]
        public string Password { get; set; }

        [MaxLength(256)]
        public string ComfirmPassword { get; set; }
    }
}
