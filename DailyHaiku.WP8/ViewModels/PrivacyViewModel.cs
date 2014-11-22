using System.Windows.Media;
using Caliburn.Micro;
using DailyHaiku.WP8.Assets;
using DailyHaiku.WP8.Resources;

namespace DailyHaiku.WP8.ViewModels
{
    public class PrivacyViewModel : Screen
    {
        private readonly BackgroundImageBrush backgroundImageBrush;

        public PrivacyViewModel(BackgroundImageBrush backgroundImageBrush)
        {
            this.backgroundImageBrush = backgroundImageBrush;
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
            get { return AppResources.PrivacyPageTitle; }
        }
    }
}
