using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using DailyHaiku.WP8.Twitter;
using DailyHaiku.WP8.ViewModels;

namespace DailyHaiku.WP8.Model
{
    public class PreviousHaikusRetriever
    {
        private readonly IEventAggregator eventAggregator;
        private readonly TimeBetweenCalculator timeBetweenCalculator;
        private readonly DailyHaikyStorage dailyHaikyStorage;

        public PreviousHaikusRetriever(
            IEventAggregator eventAggregator, 
            TimeBetweenCalculator timeBetweenCalculator,
            DailyHaikyStorage dailyHaikyStorage)
        {
            this.eventAggregator = eventAggregator;
            this.timeBetweenCalculator = timeBetweenCalculator;
            this.dailyHaikyStorage = dailyHaikyStorage;
        }

        public void GetPrevious()
        {
            ThreadPool.QueueUserWorkItem(LoadAll);
        }

        private void LoadAll(object state)
        {
            Haiku[] previousHaikus = dailyHaikyStorage.GetPreviousHaikus();
            CalculateTimeBetween(previousHaikus);
            var previousHaikusReceivedEvent = new PreviousHaikusReceivedEvent();
            previousHaikusReceivedEvent.Haikus = previousHaikus.OrderBy(h => h.TimeAfterTweet).ToList();
            eventAggregator.Publish(previousHaikusReceivedEvent);
       }

        private void CalculateTimeBetween(IEnumerable<Haiku> previousHaikus)
        {
            foreach (var previousHaiku in previousHaikus)
            {
                previousHaiku.TimeAfterTweetLabel = timeBetweenCalculator.CreateTimeAfterLabel(previousHaiku.CreatedAt);
                previousHaiku.TimeAfterTweet = timeBetweenCalculator.CreateTimeAfter(previousHaiku.CreatedAt);
            }
        }
    }
}
