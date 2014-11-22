using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using DailyHaiku.WP8.Assets;
using DailyHaiku.WP8.Model;
using DailyHaiku.WP8.Twitter;

namespace DailyHaiku.WP8.ViewModels
{
    public class PreviousHaikusViewModel : Screen, IHandle<PreviousHaikusReceivedEvent>
    {
        private readonly BackgroundImageBrush backgroundImageBrush;
        private readonly PreviousHaikusRetriever previousHaikuReceiver;
        private readonly INavigationService navigationService;

        public PreviousHaikusViewModel(INavigationService navigationService, 
            BackgroundImageBrush backgroundImageBrush,
            IEventAggregator eventAggregator, PreviousHaikusRetriever previousHaikuReceiver)
        {
            this.navigationService = navigationService;
            this.backgroundImageBrush = backgroundImageBrush;
            this.previousHaikuReceiver = previousHaikuReceiver;
            eventAggregator.Subscribe(this);
        }

        protected override void OnViewLoaded(object view)
        {
            previousHaikuReceiver.GetPrevious();
            base.OnViewLoaded(view);
        }

        public ImageBrush Background
        {
            get { return backgroundImageBrush.GetBackground(); }
        }

        private List<Haiku> previousDailyHaikus;

        public List<Haiku> PreviousDailyHaikus
        {
            get { return previousDailyHaikus; }
            set
            {
                previousDailyHaikus = value;
                NotifyOfPropertyChange(() => PreviousDailyHaikus);
            }
        }

        public void Handle(PreviousHaikusReceivedEvent message)
        {
            PreviousDailyHaikus = message.Haikus;
        }
    }
}
