using System;

namespace DailyHaiku.WP8.Common
{
    public class FiveHundredPixQueryCreator
    {
        public string Create(string imageTag)
        {
            string path = "/v1/photos/search?image_size=4&rpp=1&tag=" + imageTag;
            string url = String.Format("https://api.500px.com{0}&sdk_key={1}", path, FiveHunderdPixConstants.JavascriptSdkKey);
            return url;
        }
    }
}