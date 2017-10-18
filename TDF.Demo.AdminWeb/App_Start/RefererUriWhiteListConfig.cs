using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TDF.Core.Log;

namespace TDF.Demo.AdminWeb
{
    /// <summary>
    /// 域名请求白名单配置
    /// </summary>
    public static class RefererUriWhiteListConfig
    {
        /// <summary>
        /// 域名请求白名单
        /// 已经有初始默认值，如需补充 请在配置文件中配置额外域名白名单
        /// [配置格式 Key:"System.RefererUriWhiteList", 值格式：域名字符串(只要域名或者IP，不要有[http:// 或 https://])，以英文逗号分隔]
        /// 额外域名白名单加载 请在Web  Global.asax.cs 中进行 
        /// </summary>
        public static List<string> RefererUriWhiteList = new List<string>()
        {
            "localhost",
            "127.0.0.1",
            "192.168.10.16",//T2M 内部局域网IP
            "121.40.211.95", //T2M 开发测试服务器
            "10.0.2.40", //TCL大学内网IP
            "xuetang.tcl.com" //TCL大学正式域名
        };

        /// <summary>
        /// 加载额外请求域名白名单
        /// </summary>
        public static void LoadExtraRefererUri()
        {
            try
            {
                var value = ConfigurationManager.AppSettings["System.RefererUriWhiteList"];
                if (string.IsNullOrEmpty(value))
                    return;

                value = value.Trim().ToLower();
                var uriList = value.Split(',');
                foreach (var uri in uriList)
                {
                    RefererUriWhiteList.Add(uri);
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error("加载额外请求白名单时出错==>" + ex.Message);
            }
        }
    }
}