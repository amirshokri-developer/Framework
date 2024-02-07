namespace ASh.Framework.Core.Extensions
{
    public static class DateTimeExtensions
    {

        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static IEnumerable<DateTime> GetRangeTo(this DateTime startTime,
            DateTime endTime)
        {
            if (startTime > endTime)
                return Enumerable.Empty<DateTime>();


            return Enumerable.Range(0, 1 + endTime.Subtract(startTime).Days)
                .Select(offset => startTime.AddDays(offset));

        }

        public static long ToUnix(this DateTime dateTime)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan unixTimeSpan = dateTime - unixEpoch;

            return (long)unixTimeSpan.TotalSeconds;
        }

        public static int Age(this DateTime dateTime)
        {
            if (DateTime.Today.Month < dateTime.Month ||
            DateTime.Today.Month == dateTime.Month &&
             DateTime.Today.Day < dateTime.Day)
            {
                return DateTime.Today.Year - dateTime.Year - 1;
            }
            else
                return DateTime.Today.Year - dateTime.Year;
        }


        public static bool IsLeapYear(this DateTime dateTime)
        {
            return (DateTime.DaysInMonth(dateTime.Year, 2) == 29);
        }


        public static bool IsBetween(this DateTime dateTime,
           DateTime startDate,
           DateTime endDate,
           bool compareTime = false)
        {
            return compareTime ?
               dateTime >= startDate && dateTime <= endDate :
               dateTime.Date >= startDate.Date && dateTime.Date <= endDate.Date;
        }

    }
}
