using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Set the offset without changing the time 
        /// For example: 2020-01-01 18:00:00 GMT+06:00 -> 2020-01-01 18:00:00 GMT+02:00)
        /// </summary>
        /// <param name="timezone">See: https://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx </param>
        public static DateTimeOffset SetOffset(this DateTime date, string timezone)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);

            return new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Unspecified), zone.GetUtcOffset(time));
        }

        /// <summary>
        /// Set the offset without changing the time 
        /// For example: 2020-01-01 18:00:00 GMT+06:00 -> 2020-01-01 18:00:00 GMT+02:00)
        /// </summary>
        public static DateTimeOffset SetOffset(this DateTime date, Locale locale)
        {
            string timezone = LocaleSettings.GetTimezone(locale);

            return date.SetOffset(timezone);
        }

        /// <param name="timezone">See: https://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx </param>
        public static DateTimeOffset ToOffset(this DateTime date, string timezone)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);

            return new DateTimeOffset(date).ToOffset(zone.GetUtcOffset(time));
        }

        public static DateTimeOffset ToOffset(this DateTime date, Locale locale)
        {
            string timezone = LocaleSettings.GetTimezone(locale);

            return date.ToOffset(timezone);
        }
    }
}
