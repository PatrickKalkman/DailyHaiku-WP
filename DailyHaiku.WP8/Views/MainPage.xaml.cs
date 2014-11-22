using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace DailyHaiku.WP8.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var askforReview = (bool)IsolatedStorageSettings.ApplicationSettings["askforreview"];
            if (askforReview)
            {
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;
                var returnvalue = MessageBox.Show("Thank you for using daily haiku, would you like to review this app?", "Please review my app", MessageBoxButton.OKCancel);
                if (returnvalue == MessageBoxResult.OK)
                {
                    var marketplaceReviewTask = new MarketplaceReviewTask();
                    marketplaceReviewTask.Show();
                }
            }
            base.OnNavigatedTo(e);
        }
    }
}