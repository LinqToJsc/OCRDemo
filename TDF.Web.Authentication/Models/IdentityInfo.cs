using System;

namespace TDF.Web.Authentication.Models
{
    /// <summary>
    /// 登录者相关信息
    /// </summary>
    public class IdentityInfo
    {
        public Guid UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpiredTime { get; set; }

        public string UserName { get; set; }
    }

    /// <summary>
    /// 登录者相关信息
    /// </summary>
    public class IdentityInfo<T> : IdentityInfo where T : new ()
    {
        public T UserInfo { get; set; }
    }
}
