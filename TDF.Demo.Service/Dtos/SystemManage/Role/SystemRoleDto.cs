using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public class SystemRoleDto : IDto
    {
        public Guid? Id { get; set; }

        public DateTime? ModifiedTime { get; set; }

        public Guid? CreatorId { get; set; }

        [MaxLength(32)]
        public string CreatorName { get; set; }

        public Guid? ModifierId { get; set; }

        [MaxLength(32)]
        public string ModifierName { get; set; }

        public DateTime? CreatedTime { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public string Desc { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "长度不能超过20个字符")]
        public string Name { get; set; }

        public bool Disabled { get; set; }
    }
}
