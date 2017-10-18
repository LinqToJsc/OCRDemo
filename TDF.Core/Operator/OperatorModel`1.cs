using System;
using System.Collections.Generic;

namespace TDF.Core.Operator
{
    public class OperatorModel
    {
        public Guid Id { get; set; }

        public List<Guid> RoleIds { get; set; }

        public string UserName { get; set; }

        public string LoginToken { get; set; }

        public DateTime LoginTime { get; set; }

        public bool IsSystem { get; set; }

        public DateTime ExpiredTime { get; set; }
    }

    public class OperatorModel<T> : OperatorModel where T : class, new()
    {
        public T UserInfo { get; set; }
    }
}
