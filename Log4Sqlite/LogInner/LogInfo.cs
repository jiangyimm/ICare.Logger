using System;

namespace Log4Sqlite.LogInner
{
    public class LogInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, NotNull]
        public DateTime Time { get; set; }

        [NotNull]
        public int Level { get; set; }

        [NotNull]
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public override string ToString()
        {
            return $"[{Time}]-{Level}:{Message}\r\n{StackTrace}";
        }
    }

   
}
