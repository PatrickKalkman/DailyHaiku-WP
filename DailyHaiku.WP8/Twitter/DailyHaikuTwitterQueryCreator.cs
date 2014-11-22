using System;

namespace DailyHaiku.WP8.Twitter
{
    public class DailyHaikuTwitterQueryCreator
    {
        public string Create()
        {
            string sinceDate = DateTime.Now.ToString("yyyy-MM-dd");
            return string.Format("https://api.twitter.com/1.1/search/tweets.json?q=%23haiku%20since:{0}&rpp=5&result_type=mixed", sinceDate);
        }
    }
}