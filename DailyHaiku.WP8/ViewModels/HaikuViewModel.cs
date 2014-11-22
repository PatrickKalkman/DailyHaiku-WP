using System;
using System.Collections.Generic;
using Caliburn.Micro;
using DailyHaiku.WP8.Assets;
using DailyHaiku.WP8.Common;
using DailyHaiku.WP8.Resources;
using DailyHaiku.WP8.Twitter;
using System.Windows.Media;
using Microsoft.Xna.Framework.GamerServices;

namespace DailyHaiku.WP8.ViewModels
{
    public class HaikuViewModel : Screen, IHandle<HaikuReceivedEvent>, IHandle<HaikuImagesReceivedEvent>
    {
        private readonly DailyHaikuRetriever dailyHaikuRetriever;
        private readonly FiveHundredPixRetriever fiveHundredPixRetriever;
        private readonly IEventAggregator eventAggregator;
        private readonly BackgroundImageBrush backgroundImageBrush;
        private readonly INavigationService navigationService;
        private readonly TermExtractor termExtractor;
        private readonly Share share;
        private readonly HaikuExtractor haikuExtractor;

        public HaikuViewModel(DailyHaikuRetriever dailyHaikuRetriever,
            FiveHundredPixRetriever fiveHundredPixRetriever,
            IEventAggregator eventAggregator,
            BackgroundImageBrush backgroundImageBrush,
            INavigationService navigationService,
            TermExtractor termExtractor,
            Share share,
            HaikuExtractor haikuExtractor)
        {
            this.dailyHaikuRetriever = dailyHaikuRetriever;
            this.fiveHundredPixRetriever = fiveHundredPixRetriever;
            this.eventAggregator = eventAggregator;
            this.backgroundImageBrush = backgroundImageBrush;
            this.navigationService = navigationService;
            this.termExtractor = termExtractor;
            this.share = share;
            this.haikuExtractor = haikuExtractor;
            this.eventAggregator.Subscribe(this);
        }

        public void Handle(HaikuReceivedEvent message)
        {
            IsLoading = false;
            TodayHaiku = message.Haiku;
            string imageTag = termExtractor.Extract(message.Haiku.Tweet);
            fiveHundredPixRetriever.RetrieveImagesWithHaiku(imageTag);
        }

        public void Handle(HaikuImagesReceivedEvent message)
        {
            ImageSource1 = new Uri(message[0]);
        }

        private Uri imageSource1;

        public Uri ImageSource1
        {
            get { return imageSource1; }
            set
            {
                imageSource1 = value;
                NotifyOfPropertyChange(() => ImageSource1);
            }
        }

        public string ApplicationName
        {
            get { return AppResources.ApplicationTitle; }
        }

        public string PageTitle
        {
            get { return AppResources.HaikuTitle; }
        }

        protected override void OnViewLoaded(object view)
        {
            IsLoading = true;
            dailyHaikuRetriever.RetrieveTodaysHaiku();
            base.OnViewLoaded(view);
        }

        private Haiku todayHaiku;

        public Haiku TodayHaiku
        {
            get { return todayHaiku; }
            set
            {
                todayHaiku = value;
                NotifyOfPropertyChange(() => TodayHaiku);
            }
        }

        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                this.NotifyOfPropertyChange(() => this.IsLoading);
            }
        }

        public ImageBrush Background
        {
            get { return backgroundImageBrush.GetBackground(); }
        }

        public void Previous()
        {
            navigationService.UriFor<PreviousHaikusViewModel>().Navigate();
        }

        public void Share()
        {
            List<string> buttons = new List<string>() { "SMS", "Social" };
            Guide.BeginShowMessageBox("Send Haiku", "Do you want to send this haiku using?", buttons, 0, MessageBoxIcon.Alert, Callback, null);
        }

        private void Callback(IAsyncResult ar)
        {
            int? result = Guide.EndShowMessageBox(ar);
            if (TodayHaiku != null)
            {
                string haiku = haikuExtractor.Extract(TodayHaiku.Tweet);
                if (result.HasValue)
                {
                    if (result == 0)
                    {
                        share.ShareSMS(haiku);
                    }
                    else if (result == 1)
                    {
                        share.ShareStatus(haiku);
                    }
                }
            }
        }
    }
}