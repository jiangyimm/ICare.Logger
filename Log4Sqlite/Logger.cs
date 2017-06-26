using System;
using Log4Sqlite.LogInner;

namespace Log4Sqlite
{
    public class Logger
    {
        public static void Debug(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Debug, obj);
        }

        public static void Info(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Info, obj);
        }

        public static void Warn(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Warn, obj);
        }

        public static void Error(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Error, obj);
        }

        public static void Fatal(LogType type, object obj)
        {
            LoggerMaker.WriteLog(type, LogLevel.Fatal, obj);
        }

        public static void DebugFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Debug, string.Format(format, args));
        }

        public static void InfoFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Info, string.Format(format, args));
        }

        public static void WarnFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Warn, string.Format(format, args));
        }

        public static void ErrorFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Error, string.Format(format, args));
        }

        public static void FatalFormat(LogType type, string format, params object[] args)
        {
            LoggerMaker.WriteLog(type, LogLevel.Fatal, string.Format(format, args));
        }

        #region[兼容处理]

        public static LoggerWrapper Main { get; set; } = new LoggerWrapper(LogType.LogMain);
        public static LoggerWrapper Net { get; set; } = new LoggerWrapper(LogType.LogNet);
        public static LoggerWrapper Device { get; set; } = new LoggerWrapper(LogType.LogDevice);
        public static LoggerWrapper Update { get; set; } = new LoggerWrapper(LogType.LogUpdate);

        #endregion
    }

    public class LoggerWrapper
    {
        private readonly LogType _logType;

        public LoggerWrapper(LogType ltype)
        {
            _logType = ltype;
        }

        public void Info(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Info, message);
        }

        public void Debug(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Warn, message);
        }

        public void Error(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Error, message);
        }

        public void Fatal(string message)
        {
            LoggerMaker.WriteLog(_logType, LogLevel.Fatal, message);
        }
    }
}