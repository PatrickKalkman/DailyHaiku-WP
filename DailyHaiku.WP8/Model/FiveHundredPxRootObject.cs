using System.Collections.Generic;

namespace DailyHaiku.WP8.Model
{
    public class Image
    {
        public string size { get; set; }
        public string url { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string fullname { get; set; }
        public string userpic_url { get; set; }
        public int upgrade_status { get; set; }
        public int followers_count { get; set; }
        public int affection { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int times_viewed { get; set; }
        public double rating { get; set; }
        public string created_at { get; set; }
        public int category { get; set; }
        public bool privacy { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int votes_count { get; set; }
        public int favorites_count { get; set; }
        public int comments_count { get; set; }
        public bool nsfw { get; set; }
        public string image_url { get; set; }
        public List<Image> images { get; set; }
        public User user { get; set; }
    }

    public class FiveHundredPxRootObject
    {
        public int current_page { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public List<Photo> photos { get; set; }
    }
}
