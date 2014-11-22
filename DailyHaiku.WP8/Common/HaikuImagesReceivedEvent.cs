using System.Collections.Generic;

namespace DailyHaiku.WP8.Common
{
    public class HaikuImagesReceivedEvent : List<string>
    {
        public void AddPhotoUrl(string imageUrl)
        {
            this.Add(imageUrl);
        }
    }
}