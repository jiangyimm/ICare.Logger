using System.Collections.Generic;
using System.IO;

namespace Log4Sqlite.LogInner
{
    public class SQLiteLoggerRepository
    {
        private static readonly Dictionary<string, SQLiteLoggerRepository> Repositories = new Dictionary<string, SQLiteLoggerRepository>();
        private readonly Dictionary<int, SQLiteLogger> _dictionary = new Dictionary<int, SQLiteLogger>();
        private readonly DirectoryInfo _dir;

        public static SQLiteLoggerRepository Get(string basePath)
        {
            lock (Repositories)
            {
                if (Repositories.ContainsKey(basePath))
                    return Repositories[basePath];
                var repo = new SQLiteLoggerRepository(basePath);
                Repositories[basePath] = repo;
                return repo;
            }
        }

        private SQLiteLoggerRepository(string basePath)
        {
            _dir = !Directory.Exists(basePath) ? Directory.CreateDirectory(basePath) : new DirectoryInfo(basePath);
        }

        public SQLiteLogger Get(int yearMonth)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(yearMonth))
                    return _dictionary[yearMonth];
                var log = new SQLiteLogger(_dir, yearMonth);
                _dictionary.Add(yearMonth, log);
                return log;
            }
        }
    }
}