using System.Collections.Generic;
using Caliburn.Micro;
using DailyHaiku.WP8.ViewModels;
using Newtonsoft.Json;

namespace DailyHaiku.WP8.Twitter
{
    public class DailyHaikuRetriever
    {
        private readonly HaikuHttpClient twitterHttpClient;
        private readonly DailyHaikuTwitterQueryCreator dailyHaikuQueryCreator;
        private readonly IEventAggregator eventAggregator;
        private readonly TimeBetweenCalculator timeBetweenCalculator;
        private readonly DailyHaikyStorage dailyHaikyStorage;

        public DailyHaikuRetriever(
            HaikuHttpClient twitterHttpClient, 
            DailyHaikuTwitterQueryCreator dailyHaikuQueryCreator, 
            IEventAggregator eventAggregator,
            TimeBetweenCalculator timeBetweenCalculator, 
            DailyHaikyStorage dailyHaikyStorage)
        {
            this.twitterHttpClient = twitterHttpClient;
            this.dailyHaikuQueryCreator = dailyHaikuQueryCreator;
            this.eventAggregator = eventAggregator;
            this.timeBetweenCalculator = timeBetweenCalculator;
            this.dailyHaikyStorage = dailyHaikyStorage;
        }

        public void RetrieveTodaysHaiku()
        {
            if (!dailyHaikyStorage.IsAlreadyAvailable())
            {
                RetrieveDailyHaikuFromTwitter();
            }
            else
            {
                HaikuReceivedEvent haikuReceivedEvent = dailyHaikyStorage.GetCurrentHaiku();
                eventAggregator.Publish(haikuReceivedEvent);
            }
        }

        private void RetrieveDailyHaikuFromTwitter()
        {
            string query = dailyHaikuQueryCreator.Create();
            twitterHttpClient.GetResponse(query, r =>
                    {
                        string twitterResponse = ((HaikuHttpClientResult) r).Response;
                        if (!string.IsNullOrEmpty(twitterResponse))
                        {
                            HaikuReceivedEvent haikuReceivedEvent = ExtractHaiku(twitterResponse);
                            dailyHaikyStorage.StoreTodaysHaiku(haikuReceivedEvent);
                            dailyHaikyStorage.AddToPreviousHaikus(haikuReceivedEvent);
                            eventAggregator.Publish(haikuReceivedEvent);
                        }
                    });
        }

        private HaikuReceivedEvent ExtractHaiku(string twitterResponse)
        {
            RootObject parsedSearchResult = JsonConvert.DeserializeObject<RootObject>(twitterResponse);
            if (parsedSearchResult != null)
            {
                if (parsedSearchResult.statuses != null && parsedSearchResult.statuses.Count > 0)
                {
                    return GetHaikuWithMostRetweets(parsedSearchResult.statuses);
                }
            }
            return null;
        }

        private HaikuReceivedEvent GetHaikuWithMostRetweets(IEnumerable<Status> results)
        {
            var haiku = new Haiku();
            var haikuReceivedEvent = new HaikuReceivedEvent();
            haikuReceivedEvent.Haiku = haiku;

            int highestNumberOfRetweets = -1;

            foreach (Status result in results)
            {
                if (result.metadata.recent_retweets > highestNumberOfRetweets)
                {
                    highestNumberOfRetweets = result.metadata.recent_retweets;
                    haikuReceivedEvent.Haiku.Tweet = result.text;
                    haikuReceivedEvent.Haiku.TwitterUserProfileImageUrl = result.user.profile_background_image_url;
                    haikuReceivedEvent.Haiku.TwitterUserName = result.username;
                    haikuReceivedEvent.Haiku.TwitterUserHandle = string.Format("@{0}", result.username);
                    haikuReceivedEvent.Haiku.TimeAfterTweetLabel = timeBetweenCalculator.CreateTimeAfterLabel(result.created_at);
                    haikuReceivedEvent.Haiku.TimeAfterTweet = timeBetweenCalculator.CreateTimeAfter(result.created_at);
                    haikuReceivedEvent.Haiku.CreatedAt = result.created_at;
                    haikuReceivedEvent.Haiku.TweetId = result.id.ToString();
                }
            }
            return haikuReceivedEvent;
        }
    }
}
 