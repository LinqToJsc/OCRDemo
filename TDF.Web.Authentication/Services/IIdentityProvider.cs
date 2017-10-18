using TDF.Web.Authentication.Models;
using TDF.Web.Models;

namespace TDF.Web.Authentication.Services
{
    /// <summary>
    /// WebApi授权控制器，用于Api的身份认证
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// 获得当前登录的用户身份信息
        /// </summary>
        /// <returns></returns>
        IdentityInfo<T> GetCurrent<T>() where T : new ();

        /// <summary>
        /// 获得当前登录的用户身份信息
        /// </summary>
        /// <returns></returns>
        IdentityInfo GetCurrent();

        /// <summary>
        /// 添加一个授权用户身份信息
        /// </summary>
        /// <param name="identity"></param>
        void AddCurrent<T>(IdentityInfo<T> identity) where T : new();

        /// <summary>
        /// 添加一个授权用户身份信息
        /// </summary>
        /// <param name="identity"></param>
        void AddCurrent(IdentityInfo identity);

        /// <summary>
        /// 移除当前登录者
        /// </summary>
        void RemoveCurrent();
    }
}
