using System;

namespace DailyHaiku.WP8.ViewModels
{
    public class TermExtractor
    {
        public string Extract(string tweet)
        {
            string longestToken = string.Empty;
            string[] tokens = tweet.Split(new[] {" ", "\n", "~"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                if (token.Length > longestToken.Length)
                {
                    longestToken = token;
                }
            }
            return longestToken.Replace("!", string.Empty).Replace("?", string.Empty);
        }
    }
}