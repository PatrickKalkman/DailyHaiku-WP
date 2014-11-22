using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;

namespace Mom.WP8.Views
{
    public partial class DefaultAd : UserControl
    {
        public DefaultAd()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var searchTask = new MarketplaceSearchTask();
            searchTask.SearchTerms = "Mom";
            searchTask.Show();
        }
    }
}
