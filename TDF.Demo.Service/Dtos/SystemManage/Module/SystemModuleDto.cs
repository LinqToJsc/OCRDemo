using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public class SystemModuleDto : IDto
    {
        public Guid? Id { get; set; }

        public string Code { get; set; }

        public string Desc { get; set; }

        [MaxLength(32)]
        public string IconClass { get; set; }

        public bool Displayed { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid ParentId { get; set; }

        [Required]
        public int Sort { get; set; }

        public bool Disabled { get; set; }

        public virtual string DisplayName
        {
            get
            {
                if (ParentId != Guid.Empty)
                {
                    return "┡---" + Name;
                }
                return Name;
            }
        }

        public IEnumerable<SystemActionDto> SystemActionDtos { get; set; }
    }
}
