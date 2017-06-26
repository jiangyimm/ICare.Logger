using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Log4MongoDB
{
    public class MongoDBTest
    {
        public static bool Connect()
        {
            var ConnectionStr = "mongodb://localhost:27017";
            var mongoClient = new MongoClient(ConnectionStr);
            var db= mongoClient.GetDatabase("student");
            var col = db.GetCollection<Message>("message");
            col.InsertOne(new Message
            {
                name = "江毅",
                sex = "男",
                age = 18
            });
           
            return true;
        }

        public class Message
        {
            public ObjectId _id;
            public string name;
            public int age;
            public string sex;
        }
    }
}
