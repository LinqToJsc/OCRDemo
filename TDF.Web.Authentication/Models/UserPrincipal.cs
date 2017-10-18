using System;
using System.Security.Principal;

namespace TDF.Web.Authentication.Models
{
    public class UserPrincipal : IPrincipal
    {

        public UserPrincipal(UserIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public virtual bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
