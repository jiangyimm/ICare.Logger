using System;
using MongoDB.Bson;

namespace Log4MongoDB.LogInner
{
    public class LogInfo
    {

        public ObjectId _id { get; set; }
        //public int Id { get; set; }

      
        public DateTime Time { get; set; }

        
        public int Level { get; set; }

        
        public string Message { get; set; }

        public string StackTrace { get; set; }

        //public override string ToString()
        //{
        //    return $"[{Time}]-{Level}:{Message}\r\n{StackTrace}";
        //}
    }

   
}
