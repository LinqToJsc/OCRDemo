using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Event;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Events
{
    /// <summary>
    /// 系统用户登陆事件
    /// </summary>
    public class SystemMemberLoggedEvent : IEvent
    {
        public SystemMember SystemMember { get; set; }

        public SystemMemberLoggedEvent(SystemMember member)
        {
            SystemMember = member;
        }

        public bool CancelBubble { get; set; }

        public int Order { get; set; }

        public bool Async { get; set; }
    }
}
