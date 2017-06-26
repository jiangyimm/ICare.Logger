using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log4MongoDB.LogInner;
using Log4Sqlite;

namespace ICare.LoggerTest
{
    class Program
    {
        static void Main(string[] args)
        {
           // var watch = Stopwatch.StartNew();
           // //Task.Factory.StartNew(() =>
           // //{
           //     var i = 100;
           //     while (i > 0)
           //     {
           //         Logger.Main.Info("test");
           //         i--;
           //     }
           // //});
           //watch.Stop();
           // var time = watch.ElapsedMilliseconds;
           //Console.WriteLine(time);
            //Log4MongoDB.MongoDBTest.Connect();
           Log4MongoDB.Logger.Update.Info("test");
           Console.ReadLine();

        }
    }
}
