using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core;
using TDF.Core.Event;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Core.Tools;
using TDF.Demo.Domain.Entities.Extensions;
using TDF.Demo.Domain.Events;

namespace TDF.Demo.Domain.Entities
{
    public partial class SystemMember
    {
        public void Login(string sessionKey)
        {
            SystemMemberLogon.LogOnCount = SystemMemberLogon.LogOnCount + 1 ?? 1;
            if (SystemMemberLogon.LastVisitTime != null)
            {
                SystemMemberLogon.PreviousVisitTime = SystemMemberLogon.LastVisitTime.ToDate();
            }
            SystemMemberLogon.SessionKey = sessionKey;
            SystemMemberLogon.LastVisitTime = DateTime.Now;
            SystemMemberLogon.UserOnLine = true;
            SystemMemberLogon.Modify();
            Ioc.Resolve<IEventPublisher>().Publish(new SystemMemberLoggedEvent(this));
        }

        public void CheckPassword(string password)
        {
            if (SystemMemberLogon.Password == password)
            {
                return;
            }
            var encryptPassword = MD5Helper.GetMD5(DESEncrypt.Encrypt(password.ToLower(), SystemMemberLogon.Secretkey).ToLower(), 32).ToLower();
            if (SystemMemberLogon.Password == encryptPassword)
            {
                return;
            }
            throw new KnownException("密码不正确，请重新输入");
        }

        public void CheckStatus()
        {
            if (EnabledMark != null && !EnabledMark.Value)
            {
                throw new KnownException("账户被系统禁用,请联系管理员");
            }
        }

        public void LoginOut()
        {
            SystemMemberLogon.UserOnLine = false;
            SystemMemberLogon.Modify();
            Ioc.Resolve<IEventPublisher>().Publish(new SystemMemberLogOffedEvent(this) { Async = true });
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            var secretkey = Common.BuildKey();
            var encryptPassword = MD5Helper.GetMD5(DESEncrypt.Encrypt(password.ToLower(), secretkey).ToLower(), 32).ToLower();
            if (SystemMemberLogon != null)
            {
                SystemMemberLogon.Password = encryptPassword;
                SystemMemberLogon.Secretkey = secretkey;
            }
            else
            {
                SystemMemberLogon = new SystemMemberLogon()
                {
                    SystemMember = this,
                    Id = Guid.NewGuid(),
                    Password = encryptPassword,
                    Secretkey = secretkey
                };
                SystemMemberLogon.CreateByOperator();
            }
        }

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="roleIds">角色Id</param>
        public void AssignmentRole(List<Guid> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var memberRole = new SystemMemberRole()
                {
                    SystemMemberId = Id,
                    SystemRoleId = roleId
                };
                memberRole.CreateByOperator();
                SystemMemberRoles.Add(memberRole);
            }
        }
    }
}
