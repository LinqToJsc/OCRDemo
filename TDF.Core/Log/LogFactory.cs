using log4net;
using System;

namespace TDF.Core.Log
{
    public class LogFactory
    {
        public static ILog GetLogger(Type type)
        {
            return GetLog(type);
        }

        public static ILog GetLogger(string str = "")
        {
            return GetLog(str);
        }

        private static ILog GetLog(object type)
        {
            if (LogProvider.GetLogType().ToLower() == "EmptyLog".ToLower())
            {
                return new EmptyLog();
            }

            switch (LogProvider.GetLogType().ToLower())
            {
                case "log4net":
                    var isType = type is Type;
                    return isType ? LogManager.GetLogger((Type)type) : LogManager.GetLogger((string)type);
                default:
                    return new EmptyLog();
            }
        }
    }
}
