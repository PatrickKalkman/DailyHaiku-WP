using System;
using System.Globalization;

namespace DailyHaiku.WP8.Twitter
{
    public class TwitterDateTimeParser
    {
        public DateTime ParseDateTime(string createdAt)
        {
            return createdAt.ParseTwitterTime();
        }
    }
}
