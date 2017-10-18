using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public static class MemberDtoExtensions
    {
        public static IQueryable<SystemMemberDto> ToDtos(this IQueryable<SystemMember> query)
        {
            return query.Select(x => new SystemMemberDto()
            {
                Id = x.Id,
                Account = x.Account,
                RealName = x.RealName,
                Password = string.Empty,
                MobilePhone = x.MobilePhone,
                EnabledMark = x.EnabledMark,
                IsSuperAdmin = x.IsSuperAdmin,
                Email = x.Email,
                QQ = x.QQ,
                Gender = x.Gender
            });
        }
    }
}
