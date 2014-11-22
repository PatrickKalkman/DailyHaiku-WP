using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using DailyHaiku.WP8.Model;
using DailyHaiku.WP8.Twitter;
using Newtonsoft.Json;

namespace DailyHaiku.WP8.Common
{
    public class FiveHundredPixRetriever
    {
        private readonly HaikuHttpClient httpClient;
        private readonly FiveHundredPixQueryCreator fiveHundredPixQueryCreator;
        private readonly IEventAggregator eventAggregator;

        public FiveHundredPixRetriever(
            HaikuHttpClient httpClient,
            FiveHundredPixQueryCreator fiveHundredPixQueryCreator,
            IEventAggregator eventAggregator)
        {
            this.httpClient = httpClient;
            this.fiveHundredPixQueryCreator = fiveHundredPixQueryCreator;
            this.eventAggregator = eventAggregator;
        }

        public void RetrieveImagesWithHaiku(string imageTag)
        {
            string query = fiveHundredPixQueryCreator.Create(imageTag);
            httpClient.GetResponse(query, r =>
            {
                string fiveHundredPixResponse = ((HaikuHttpClientResult)r).Response;
                HaikuImagesReceivedEvent haikuImagesReceivedEvent = ExtractPictures(fiveHundredPixResponse);
                if (haikuImagesReceivedEvent != null)
                {
                    eventAggregator.Publish(haikuImagesReceivedEvent);
                }
            });
        }

        private HaikuImagesReceivedEvent ExtractPictures(string twitterResponse)
        {
            if (!string.IsNullOrEmpty(twitterResponse))
            {
                FiveHundredPxRootObject parsedSearchResult =
                    JsonConvert.DeserializeObject<FiveHundredPxRootObject>(twitterResponse);
                if (parsedSearchResult != null)
                {
                    if (parsedSearchResult.photos != null && parsedSearchResult.photos.Count > 0)
                    {
                        var haikuImagesReceivedEvent = new HaikuImagesReceivedEvent();
                        foreach (Photo photo in parsedSearchResult.photos)
                        {
                            haikuImagesReceivedEvent.AddPhotoUrl(photo.image_url);
                        }
                        return haikuImagesReceivedEvent;
                    }
                }
            }
            return null;
        }
    }
}
