using System;
using System.Windows;
using System.Windows.Controls;
using DailyHaiku.WP8.ViewModels;

namespace DailyHaiku.WP8.Views
{
    public partial class HaikuItemView : UserControl
    {
        public HaikuItemView()
        {
            InitializeComponent();
            this.TwitterUserProfileImage.Loaded += TwitterUserProfileImageOnLoaded;
            this.Loaded += HaikuItemView_Loaded;
        }

        private void TwitterUserProfileImageOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            FadeInStoryBoard.Begin();
        }

        void HaikuItemView_Loaded(object sender, RoutedEventArgs e)
        {
            TwitterUserProfileImage.Opacity = 0;
        }

        public static readonly DependencyProperty HaikuProperty = DependencyProperty.Register
            ("Haiku", 
            typeof(Haiku),
            typeof(HaikuItemView), new PropertyMetadata(null, new PropertyChangedCallback(TheHaikuProperty_Changed)));


        private static void TheHaikuProperty_Changed(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            Haiku newValue = e.NewValue as Haiku;
            if (newValue != null)
            {
                var itemView = source as HaikuItemView;
                if (itemView != null)
                {
                    itemView.TwitterUserName.Text = newValue.TwitterUserName;
                    itemView.TwitterUserHandle.Text = newValue.TwitterUserHandle;
                    itemView.TwitterUserProfileImageSource.UriSource = new Uri(newValue.TwitterUserProfileImageUrl);
                    itemView.Tweet.Text = newValue.Tweet;
                    itemView.TimeAfterTweet.Text = newValue.TimeAfterTweetLabel;
                }
            }
        }

        public Haiku Haiku
        {
            get { return (Haiku)GetValue(HaikuProperty); }
            set { SetValue(HaikuProperty, value); }
        }




    }
}
