using System;
using System.Configuration;

namespace TDF.Core.Configuration
{
    public class Configs : ISettings
    {

        private static Configs _configs;

        public static Configs Instance
        {
            get
            {
                if (_configs == null)
                {
                    try
                    {
                        _configs = Ioc.Ioc.Resolve<Configs>();
                    }
                    catch (Exception)
                    {
                        return new Configs();
                    }
                }
                return _configs;
            }
        }

        private string _connectionString;

        //private string _mappingNamespace;

        private EnvironmentType? _environmentType;

        private readonly object _locked = new object();

        public virtual string ConfigMode => "配置文件";

        public virtual string ConfigFilePath => "~/Configs/system.config";

        /// <summary>
        /// 验证码key
        /// </summary>
        public virtual string Verifykey => "verifykey";

        /// <summary>
        /// Session
        /// </summary>
        public virtual string SessionKey => "__t2m_session_key";

        /// <summary>
        /// SessionKey过期分钟数
        /// </summary>
        public virtual int SessionExpireMinute => 30;

        /// <summary>
        /// TokenKey过期分钟数
        /// </summary>
        public virtual int TokenExpireMinute => 30;

        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public virtual bool EnabledVerifyCode
        {
            get
            {
                var result = GetValue("EnabledVerifyCode");
                return result == "1" || result.ToLower() == "true";
            }
        }

        /// <summary>
        /// 环境类型，开发/测试/正式
        /// </summary>
        public virtual EnvironmentType EnvironmentType {
            get
            {
                if (_environmentType != null) return _environmentType.Value;
                EnvironmentType tempType;
                _environmentType = Enum.TryParse(GetValue("EnvironmentType"), out tempType) ? tempType : EnvironmentType.Dev;
                return _environmentType.Value;
            }
            set
            {
                _environmentType = value;
            }
        }

        public virtual string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["TDFDatabase"].ConnectionString;
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public bool DatabaseInitCompleted { get; set; }

        public string InitSqlPath { get; set; }

        public static int PageSize { get; set; } = 50;

        public string GetValue(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        }

        public void SetValue(string key, string value)
        {
            lock (_locked)
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var cNode = config.AppSettings.Settings[key];
                if (cNode != null)
                {
                    cNode.Value = value;
                }
                else
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        
    }
}
