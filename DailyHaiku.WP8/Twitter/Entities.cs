using System.Collections.Generic;

namespace DailyHaiku.WP8.Twitter
{
    public class Entities
    {
        public List<object> urls { get; set; }
        public List<Hashtag> hashtags { get; set; }
        public List<object> user_mentions { get; set; }
    }
}
