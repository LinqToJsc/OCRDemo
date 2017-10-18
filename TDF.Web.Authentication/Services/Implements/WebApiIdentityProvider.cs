using System;
using System.Web;
using TDF.Core.Caching;
using TDF.Core.Configuration;
using TDF.Core.Tools;
using TDF.Web.Authentication.Attributes.WebApi;
using TDF.Web.Authentication.Models;
using TDF.Web.Models;

namespace TDF.Web.Authentication.Services.Implements
{
    /// <summary>
    /// 使用缓存的方式来暂存授权者信息
    /// </summary>
    public class WebApiIdentityProvider : IIdentityProvider
    {
        public const string ApiTokenKeyNamePrefix = "ApiTokenKey-";

        public virtual string Token => HttpContext.Current.Items[TokenValidateAttribute.TokenKeyName].ToString();

        public void AddCurrent<T>(IdentityInfo<T> user) where T: new ()
        {
            if (string.IsNullOrEmpty(user.Token))
            {
                user.Token = Common.BuildKey();
            }
            CacheManager.Set(ApiTokenKeyNamePrefix + user.Token, user, Configs.Instance.TokenExpireMinute);
        }

        public void AddCurrent(IdentityInfo user)
        {
            if (string.IsNullOrEmpty(user.Token))
            {
                user.Token = Common.BuildKey();
            }
            CacheManager.Set(ApiTokenKeyNamePrefix + user.Token, user, Configs.Instance.TokenExpireMinute);
        }

        public IdentityInfo<T> GetCurrent<T>() where T : new()
        {
            var tokenKey = ApiTokenKeyNamePrefix + Token;
            var identity = HttpContext.Current.Items[tokenKey];
            if (identity == null)
            {
                identity = CacheManager.Get<IdentityInfo<T>>(tokenKey);
                if (identity != null)
                {
                    HttpContext.Current.Items[tokenKey] = identity;
                }
                else
                {
                    return null;
                }
            }
            var identityInfo = (IdentityInfo<T>)identity;
            if ((identityInfo.ExpiredTime - DateTime.Now).Minutes < 10)
            {
                identityInfo.ExpiredTime = DateTime.Now.AddMinutes(Configs.Instance.TokenExpireMinute);
                CacheManager.Set(tokenKey, identityInfo, Configs.Instance.TokenExpireMinute);
            }
            return identityInfo;
        }

        public IdentityInfo GetCurrent()
        {
            var tokenKey = ApiTokenKeyNamePrefix + Token;
            var identity = HttpContext.Current.Items[tokenKey];
            if (identity == null)
            {
                identity = CacheManager.Get<IdentityInfo>(tokenKey);
                if (identity != null)
                {
                    HttpContext.Current.Items[tokenKey] = identity;
                }
                else
                {
                    return null;
                }
            }
            var identityInfo = (IdentityInfo)identity;
            if ((identityInfo.ExpiredTime - DateTime.Now).Minutes < 10)
            {
                identityInfo.ExpiredTime = DateTime.Now.AddMinutes(Configs.Instance.TokenExpireMinute);
                CacheManager.Set(tokenKey, identityInfo, Configs.Instance.TokenExpireMinute);
            }
            return identityInfo;
        }

        public void RemoveCurrent()
        {
            CacheManager.Remove(ApiTokenKeyNamePrefix + Token);
        }
    }
}
