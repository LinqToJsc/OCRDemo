using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Enums;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public class SystemActionDto : IDto
    {
        public Guid? Id { get; set; }

        public string Code { get; set; }

        public string Desc { get; set; }

        public bool Displayed { get; set; }

        [Required, MaxLength(16)]
        public string Name { get; set; }

        [Required]
        public int Sort { get; set; }

        public int ModuleSort { get; set; }

        public bool Disabled { get; set; }

        [Required, MaxLength(250)]
        public string Url { get; set; }

        public string ModuleName { get; set; }

        [Required]
        public Guid ModuleId { get; set; }

        public SystemModuleDto SystemModule { get; set; }

        public bool IsDelete { get; set; }

        public SystemActionType ActionType { get; set; }

        public Guid ActionParentId { get; set; }

        public string DisplayName {
            get
            {
                if (ActionType == SystemActionType.Page)
                    return Name;
                return "┡---"  + Name;
            }
        }

        public string ActionTypeStr => ActionType.GetDisplayName();

        public string ShowStr => Displayed?"显示":"隐藏";

        public string StateStr => Disabled ? "禁用" : "启用";

        public virtual string ActionPageCommbox { get; set; }
    }
}
