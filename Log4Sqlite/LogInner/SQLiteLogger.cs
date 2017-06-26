using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Log4Sqlite.LogInner
{
    public class SQLiteLogger : IDisposable
    {
        private readonly string dbname;
        private SQLiteConnection db;
        public int YearMonth { get; }
        private static object _lock = new object();
        private static UdpClient udpClient = new UdpClient { MulticastLoopback = true };
        private static LogPacket logPacket = new LogPacket();
        private static AutoResetEvent sendMulticastMessageEvent = new AutoResetEvent(false);
        private bool isConnect = false;
        public SQLiteLogger(DirectoryInfo dir, int yearMonth)
        {
            if (!dir.Exists)
                dir.Create();

            YearMonth = yearMonth;
            dbname = Path.Combine(dir.FullName, $"YTLog {yearMonth / 100}-{yearMonth % 100:D2}.db");
            db = new SQLiteConnection(dbname);
            CreateTables();
            //Task.Factory.StartNew(SendMultiCastMessage);
        }

        private void CreateTables()
        {
            foreach (var name in Enum.GetNames(typeof(LogType)))
            {
                db.CreateTable<LogInfo>(name);
            }
        }

        public List<T> Query<T>(long id, DateTime? startTime, int limit)
            where T : LogInfo, new()
        {
            var query = db.Table<T>().Where(info => info.Id > id);

            if (startTime != null)
                query = query.Where(info => info.Time > startTime.Value);

            return query.Take(limit).OrderBy(info => info.Id).ToList();
        }

        public long Max<T>()
            where T : LogInfo, new()
        {
            return db.Query<T>($"SELECT `Id` FROM `{typeof(T).Name}` ORDER BY Id DESC LIMIT 1;").FirstOrDefault()?.Id ?? 0;
        }

        public bool Insert<T>(T info)
            where T : LogInfo
        {
            try
            {
                lock (_lock)
                {

                    db.Insert(info);
                }
                return true;
            }
            catch (Exception)
            {
                //Logger.log.Error("写数据库错误:" + e.Message + e.StackTrace);
                return false;
            }
        }

        public bool Insert<T>(IEnumerable<T> info)
            where T : LogInfo
        {
            try
            {
                lock (_lock)
                {
                    db.InsertAll(info);
                }
                return true;
            }
            catch (Exception)
            {
                //Logger.log.Error("写数据库错误:" + e.Message + e.StackTrace);
                return false;
            }
        }

        public bool Insert<T>(string tableName, T[] info)
           where T : LogInfo
        {

            try
            {
                lock (_lock)
                {
                    if (info == null || info.Length == 0)
                    {
                        return false;
                    }
                    if (info.Length == 1)
                    {
                        db.Insert(tableName, info.FirstOrDefault());
                    }
                    else
                    {
                        db.InsertAll(tableName, info);
                    }
                    //logPacket.Name = tableName;
                    //logPacket.IP = NetworkManager.IP;
                    //logPacket.LogAllInfo = JsonConvert.SerializeObject(info);
                    //sendMulticastMessageEvent.Set();
                }
                return true;
            }
            catch (Exception)
            {
                //Logger.log.Error("写数据库错误:" + e.Message + e.StackTrace);
                return false;
            }
        }

        public bool Update<T>(T info)
        {
            try
            {
                db.Update(info);
                return true;
            }
            catch (Exception)
            {
                //Logger.log.Error("写数据库错误:" + e.Message + e.StackTrace);
                return false;
            }
        }

        private void SendMultiCastMessage()
        {
            while (true)
            {
                if (!isConnect)
                {
                    if (string.IsNullOrEmpty(FrameworkConst.MultiCastIP) || string.IsNullOrEmpty(FrameworkConst.MultiCastPort))
                    {
                        continue;
                    }
                    var address = IPAddress.Parse(FrameworkConst.MultiCastIP);
                    var port = int.Parse(FrameworkConst.MultiCastPort);
                    var multicast = new IPEndPoint(address, port);
                    udpClient.JoinMulticastGroup(multicast.Address);
                    udpClient.Connect(multicast);
                    isConnect = true;
                }
                sendMulticastMessageEvent.WaitOne();
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(logPacket));
                udpClient.Send(bytes, bytes.Length);
            }
        }

        ~SQLiteLogger()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ((IDisposable)db).Dispose();
        }

    }

    public class LogPacket
    {
        public string Name { get; set; }

        public string IP { get; set; }

        public string LogAllInfo { get; set; }
    }
}
