using System.Security.Principal;
using TDF.Web.Models;

namespace TDF.Web.Authentication.Models
{
    public class UserIdentity : IIdentity
    {
        public UserIdentity(IdentityInfo user)
        {
            IsAuthenticated = true;
            Name = user.UserName;
            User = user;

        }
        public string AuthenticationType
        {
            get
            {
                return "CustomAuthentication";
            }
        }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }

        public IdentityInfo User { get; private set; }
    }
}
