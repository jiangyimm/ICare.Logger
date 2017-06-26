using MongoDB.Driver;
using System;
using MongoDB.Bson;

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
            if(connected)
                return;
            mongoClient = new MongoClient("mongodb://localhost:27017");
            db = mongoClient.GetDatabase("ICarelog2");
            CreateTables();
            connected = true;
        }
        private static void CreateTables()
        {
            foreach (var name in Enum.GetNames(typeof(LogType)))
            {
                try
                {
                    db.CreateCollection(name,new CreateCollectionOptions());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                   
                }
                
            }
        }

        public static void Insert(LogType logType, LogLevel logLevel,object logMessage)
        {
            Init();
            var info=new LogInfo
            {
                Level = (int)logLevel,
                Message = logMessage.ToString()
            };

            var doc=new BsonDocument
            {
                { "Level",(int)logLevel},
                {"Message",logMessage.ToString() }
            };
            var name = logType.ToString();
            var col = db.GetCollection<LogInfo>(name);
            col.InsertOne(info);
        }
    }
}