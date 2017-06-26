namespace Log4Sqlite.LogInner
{
    internal interface ILogAppender
    {
        void Insert(SQLiteLogger logger, LogInfo[] infos);
    }
}