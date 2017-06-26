using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using log4net.Appender;
using log4net.Core;

namespace Log4Sqlite.LogInner
{
    internal class SQLiteAppender : BufferingAppenderSkeleton
    {
        private static readonly SQLiteLogger Logger;

        private bool _initialized;
        private ILogAppender _appender;
        static SQLiteAppender()
        {
            var repository = SQLiteLoggerRepository.Get(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
            Logger = repository.Get(DateTimeCore.Today.ToYearMonth());
        }

        public SQLiteAppender()
        {
        }

        public SQLiteAppender(string name, int bufferSize = 16
//#if DEBUG
//            int bufferSize = 1
//#else
//            int bufferSize = 16
//#endif
            )
        {
            Name = name;
            BufferSize = bufferSize;
        }

        private void Initialize()
        {
            _appender=new StaticLogAppender(Name);
            _initialized = true;
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (!_initialized)
                Initialize();
            _appender.Insert(Logger, events.Select(Convert).ToArray());
        }

        private LogInfo Convert(LoggingEvent loggingEvent)
        {
            return new LogInfo
            {
                Time = loggingEvent.TimeStamp,
                Level = loggingEvent.Level.Value,
                Message = loggingEvent.RenderedMessage,
                StackTrace = loggingEvent.Level.Value>Level.Info.Value?(new StackTrace(16)).ToString():""
            };
        }
    }
}