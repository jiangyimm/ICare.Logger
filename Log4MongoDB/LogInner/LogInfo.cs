using System;

namespace Log4MongoDB.LogInner
{
    public class LogInfo : InfoBase
    {
        public DateTime DateTime { get; set; }

        public int Level { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}