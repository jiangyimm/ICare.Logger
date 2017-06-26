using System;

namespace Log4Sqlite.LogInner
{
    //todo 同步时间服务器等操作
    public class DateTimeCore
    {
        private static TimeSpan _centerSpan; //中心与本地的时间差
        private static DateTime _oldLocalTime; //记录上次请求时，本地的时间，两次请求之间的时间进行比较，如果大于1天，则需要重新校正

        public static Func<DateTime> TimeUpdateFunc;

        public static DateTime Now
        {
            get
            {
                // var now = DateTime.Now;
                //if (_oldLocalTime == DateTime.MinValue)
                //{
                //    _oldLocalTime = now;
                //}
                //else if ((now - _oldLocalTime).TotalDays > 1) //差距超过一天，可能本地时间已经被修改，重新发起时间更改请求
                //{
                //    var interDate = TimeUpdateFunc?.Invoke() ?? DateTime.Now;
                //    _centerSpan = DateTime.Now - interDate;
                //}
                return /*_oldLocalTime =*/ DateTime.Now.AddMilliseconds(_centerSpan.TotalMilliseconds);
            }
            set { _centerSpan = value - DateTime.Now; }
        }

        public static DateTime Today => Now.Date;
    }
}