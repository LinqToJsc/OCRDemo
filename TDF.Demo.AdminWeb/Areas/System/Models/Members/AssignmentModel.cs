using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TDF.Demo.Service.Dtos.SystemManage;

namespace TDF.Demo.AdminWeb.Areas.System.Models.Members
{
    public class AssignmentModel
    {
        public List<SystemRoleDto> NotInRoles { get; set; }

        public List<SystemRoleDto> InRoles { get; set; }

        public Guid MemberId { get; set; }

        public string UserName { get; set; }

    }
}