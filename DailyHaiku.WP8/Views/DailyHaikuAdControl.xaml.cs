using System.Windows;
using System.Windows.Controls;

namespace DailyHaiku.WP8.Views
{
    public partial class DailyHaikuAdControl : UserControl
    {
        public DailyHaikuAdControl()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            MyAdRotator.DefaultHouseAdBody = "DailyHaiku.WP8.Views.DefaultAd";
            MyAdRotator.Invalidate(true);
        }
    }
}
