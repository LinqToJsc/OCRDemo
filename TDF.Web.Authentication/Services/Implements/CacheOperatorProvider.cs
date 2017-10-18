using System;
using System.Web;
using TDF.Core.Caching;
using TDF.Core.Configuration;
using TDF.Core.Operator;
using TDF.Core.Tools;
using TDF.Web.Infrastructure;

namespace TDF.Web.Authentication.Services.Implements
{
    /// <summary>
    /// 使用缓存的方式
    /// </summary>
    public class CacheOperatorProvider : IOperatorProvider
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// {0}-随机密钥
        /// </summary>
        public const string CACHE_OPERATOR_PROVIDER_KEY = "loginuserkey_{0}";

        public OperatorModel GetCurrent()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            var key = WebHelper.GetCookie(Configs.Instance.SessionKey);
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            var oper = HttpContext.Current.Items[key];
            if (oper == null)
            {
                oper = CacheManager.Get<OperatorModel>(string.Format(CACHE_OPERATOR_PROVIDER_KEY, key));
                if (oper != null)
                {
                    HttpContext.Current.Items[key] = oper;
                }
                else
                {
                    return null;
                }
            }
            var operatorModel = (OperatorModel)oper;
            if ((operatorModel.ExpiredTime - DateTime.Now).Minutes < 10)
            {
                operatorModel.ExpiredTime = DateTime.Now.AddMinutes(Configs.Instance.SessionExpireMinute);
                CacheManager.Set(string.Format(CACHE_OPERATOR_PROVIDER_KEY, key), operatorModel, Configs.Instance.SessionExpireMinute);
                WebHelper.WriteCookie(Configs.Instance.SessionKey, key, Configs.Instance.SessionExpireMinute);
            }
            return operatorModel;
        }
        
        public void AddCurrent(OperatorModel operatorModel)
        {
            var key = Common.BuildKey();
            CacheManager.Set(string.Format(CACHE_OPERATOR_PROVIDER_KEY,key),operatorModel, Configs.Instance.SessionExpireMinute);
            WebHelper.WriteCookie(Configs.Instance.SessionKey, key, Configs.Instance.SessionExpireMinute);
            operatorModel.LoginToken = key;
        }

        public void RemoveCurrent()
        {
            var key = WebHelper.GetCookie(Configs.Instance.SessionKey);
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            CacheManager.Remove(string.Format(CACHE_OPERATOR_PROVIDER_KEY, key));
        }

        public T GetCurrent<T>() where T : OperatorModel
        {
            var key = WebHelper.GetCookie(Configs.Instance.SessionKey);
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            var oper = HttpContext.Current.Items[key];
            if (oper == null)
            {
                oper = CacheManager.Get<T>(string.Format(CACHE_OPERATOR_PROVIDER_KEY, key));
                if (oper != null)
                {
                    HttpContext.Current.Items[key] = oper;
                }
                else
                {
                    return null;
                }
            }
            var operatorModel = (T)oper;
            if ((operatorModel.ExpiredTime - DateTime.Now).Minutes < 10)
            {
                operatorModel.ExpiredTime = DateTime.Now.AddMinutes(Configs.Instance.SessionExpireMinute);
                CacheManager.Set(string.Format(CACHE_OPERATOR_PROVIDER_KEY, key), operatorModel, Configs.Instance.SessionExpireMinute);
                WebHelper.WriteCookie(Configs.Instance.SessionKey, key, Configs.Instance.SessionExpireMinute);
            }
            return operatorModel;
        }

        public void AddCurrent<T>(T operatorModel) where T : OperatorModel, new()
        {
            var key = Common.BuildKey();
            CacheManager.Set(string.Format(CACHE_OPERATOR_PROVIDER_KEY, key), operatorModel, Configs.Instance.SessionExpireMinute);
            WebHelper.WriteCookie(Configs.Instance.SessionKey, key, Configs.Instance.SessionExpireMinute);
            operatorModel.LoginToken = key;
        }
    }
}
