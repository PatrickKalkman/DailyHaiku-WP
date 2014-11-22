namespace DailyHaiku.WP8.ViewModels
{
    public class HaikuExtractor
    {
        public string Extract(string tweet)
        {
            string result = tweet.Replace("#haiku", string.Empty);
            result = result.Replace("#micropoetry", string.Empty);
            result = result.Replace("#haikureply", string.Empty);
            result = result.Replace("\n", ", ");
            return result;
        }
    }
}