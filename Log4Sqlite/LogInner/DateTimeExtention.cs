using System;

namespace Log4Sqlite.LogInner
{
    public static class DateTimeExtention
    {
        public static int ToYearMonth(this DateTime date)
        {
            return date.Year*100+date.Month;
        }
    }
}
