using System;
using System.Windows.Media;
using Caliburn.Micro;
using DailyHaiku.WP8.Assets;
using DailyHaiku.WP8.Resources;
using DailyHaiku.WP8.Twitter;

namespace DailyHaiku.WP8.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private readonly INavigationService navigationService;
        private readonly BackgroundImageBrush backgroundImageBrush;

        public MainPageViewModel(INavigationService navigationService, BackgroundImageBrush backgroundImageBrush)
        {
            this.navigationService = navigationService;
            this.backgroundImageBrush = backgroundImageBrush;
        }

        public void Start()
        {
            navigationService.UriFor<HaikuViewModel>().Navigate();
        }

        public void Privacy()
        {
            navigationService.UriFor<PrivacyViewModel>().Navigate();
        }

        public void About()
        {
            navigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }
        
        public ImageBrush Background
        {
            get { return backgroundImageBrush.GetBackground(); }
        }

        public string ApplicationName
        {
            get { return AppResources.ApplicationTitle; }
        }

        public string PageTitle
        {
            get { return AppResources.MainPageTitle; }
        }

    }
}
