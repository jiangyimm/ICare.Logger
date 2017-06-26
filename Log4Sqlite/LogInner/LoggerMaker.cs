using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Log4Sqlite.Properties;
using Newtonsoft.Json;

namespace Log4Sqlite.LogInner
{
    public class LoggerMaker
    {
        static LoggerMaker()
        {
            CoreFileCopy();
            foreach (var s in Enum.GetNames(typeof(LogType)))
            {
                var repo = LogManager.CreateRepository(s);
                var appender = new SQLiteAppender(s);
                appender.ActivateOptions();

                var layout = new PatternLayout()
                {
                    ConversionPattern =
                        "[%date] [%thread] %-5level %logger [%property{NDC}] - %message%newline",
                };
                var rfAppender = new RollingFileAppender()
                {
                    Name = s,
                    File = $"Log/Text/{s}.log",
                    PreserveLogFileNameExtension = true,
                    StaticLogFileName = false,

                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    MaxFileSize = 10 * 1024 * 1024,
                    MaxSizeRollBackups = 10,

                    AppendToFile = true,
                    Layout = layout
                };
                layout.ActivateOptions();
                rfAppender.ActivateOptions();
                BasicConfigurator.Configure(repo, appender, rfAppender);
            }
        }

        private static void CoreFileCopy()
        {
            const string name = "sqlite3.dll";
            var in64Bit = IntPtr.Size == 8;
            var bytes = in64Bit ? Resources.sqlite3_X64 : Resources.sqlite3_X86;

            var fileInfo = new FileInfo("sqlite3.dll");
            if (!fileInfo.Exists || fileInfo.Length != bytes.LongLength)
                File.WriteAllBytes(name, bytes);
        }

        public static void WriteLog(LogType type, LogLevel level, object message)
        {
            var logger = LogManager.GetLogger(type.ToString(), type.ToString());
            switch (level)
            {
                case LogLevel.Debug:
                    logger.Debug(GetMessageString(message));
                    break;

                case LogLevel.Info:
                    logger.Info(GetMessageString(message));
                    break;

                case LogLevel.Warn:
                    logger.Warn(GetMessageString(message));
                    break;

                case LogLevel.Error:
                    logger.Error(GetMessageString(message));
                    break;

                case LogLevel.Fatal:
                    logger.Fatal(GetMessageString(message));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        private static string GetMessageString(object message)
        {
            if (message is string)
                return message.ToString();
            try
            {
                return JsonConvert.SerializeObject(message);
            }
            catch (Exception)
            {
                return message.ToString();
            }
        }
    }
}