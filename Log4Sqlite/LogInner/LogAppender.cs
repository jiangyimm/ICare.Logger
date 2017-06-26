namespace Log4Sqlite.LogInner
{
    internal class StaticLogAppender : ILogAppender
    {
        private readonly string _name;
        internal StaticLogAppender(string tableName)
        {
            _name = tableName;
        }

        public void Insert(SQLiteLogger logger, LogInfo[] infos)
        {
            logger.Insert(_name, infos);

        }
    }
}