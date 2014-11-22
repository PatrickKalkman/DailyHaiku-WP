using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DailyHaiku.WP8.ViewModels
{
    public class Haiku
    {
        public string TwitterUserHandle { get; set; }

        public string TwitterUserName { get; set; }

        public string TwitterUserProfileImageUrl { get; set; }

        public string TimeAfterTweetLabel { get; set; }

        public long TimeAfterTweet { get; set; }

        public string Tweet { get; set; }

        public string TweetId { get; set; }

        public string CreatedAt { get; set; }
    }
}
