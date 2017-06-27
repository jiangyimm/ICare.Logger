using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Log4MongoDB.LogInner
{
    public class MongoDbLogger
    {
        private MongoDbLogger()
        {
        }

        private static MongoClient mongoClient;
        private static IMongoDatabase db;
        private static bool connected;

        private static void Init()
        {
            if (connected)
                return;
            mongoClient = new MongoClient("mongodb://localhost:27017");
            db = mongoClient.GetDatabase("ICarelog");
            CreateTables();
            connected = true;
        }

        private static void CreateTables()
        {
            foreach (var name in Enum.GetNames(typeof(LogType)))
            {
                try
                {
                    
                    db.CreateCollection(name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void Insert<T>(LogType logType, LogLevel logLevel, T logMessage)
        {
            Init();
            if (logType == LogType.LogPointTouch)
            {
                var logtype = logType.ToString();
                var collection = db.GetCollection<T>(logtype);
                collection.InsertOne(logMessage);
                return;
            }
            var info = new LogInfo
            {
                Level = (int)logLevel,
                Message = logMessage.ToString(),
                DateTime = DateTime.Now,
                StackTrace = (int)logLevel>(int)LogLevel.Info?new StackTrace(16).ToString() :null
            };
            var name = logType.ToString();
            var col = db.GetCollection<LogInfo>(name);
            col.InsertOne(info);
        }

        public static List<T> QueryList<T>(LogType logType)
        {
            Init();
            var name = logType.ToString();
            var col = db.GetCollection<T>(name);
            return col.AsQueryable().ToList();
        }
    }
}