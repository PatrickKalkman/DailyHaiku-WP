using System;
using System.Globalization;

namespace DailyHaiku.WP8.Twitter
{
    public class TimeBetweenCalculator
    {
        private readonly TwitterDateTimeParser twitterDateTimeParser;

        public TimeBetweenCalculator(TwitterDateTimeParser twitterDateTimeParser)
        {
            this.twitterDateTimeParser = twitterDateTimeParser;
        }

        public string CreateTimeAfterLabel(string createdAt)
        {
            DateTime createdDateTime = twitterDateTimeParser.ParseDateTime(createdAt);
            TimeSpan difference = CalculateDifference(createdDateTime);
            return CreateLabel(difference);
        }

        public long CreateTimeAfter(string createdAt)
        {
            DateTime createdDateTime = twitterDateTimeParser.ParseDateTime(createdAt);
            TimeSpan difference = CalculateDifference(createdDateTime);
            return (long) difference.TotalMinutes;
        }

        private static string CreateLabel(TimeSpan difference)
        {
            if (difference.Days > 0)
            {
                return string.Format("{0}d", difference.Days);
            }

            if (difference.Hours > 0)
            {
                return string.Format("{0}h", difference.Hours);
            }

            if (difference.Minutes > 0)
            {
                return string.Format("{0}m", difference.Minutes);
            }

            if (difference.Seconds > 0)
            {
                return String.Format("{0}s", difference.Seconds);
            }

            return string.Empty;
        }

        private static TimeSpan CalculateDifference(DateTime createdDateTime)
        {
            TimeSpan difference = DateTime.Now - createdDateTime;
            return difference;
        }
    }
}