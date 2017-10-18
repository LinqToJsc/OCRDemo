namespace TDF.Core.Log
{
    public class LogProvider
    {
        private static string _logType;

        public static string GetLogType()
        {
            return _logType ?? "";
        }

        public static void SetLogType(string logType)
        {
            _logType = logType;
        }
    }
}
