namespace Devin.ObjectExtensions
{
    public static class DateExtensions
    {
        public static long ToUnixTime(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static DateTime ToDateTime(this long timeStamp)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区

            return startTime.AddTicks(timeStamp * 10000000);
        }
    }
}