using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DailyHaiku.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace DailyHaiku.WP8.Views
{
    public partial class HaikuView : PhoneApplicationPage
    {
        public HaikuView()
        {
            InitializeComponent();
            this.HaikuImage1.Loaded += HaikuImage1_Loaded;
        }

        void HaikuImage1_Loaded(object sender, RoutedEventArgs e)
        {
            FadeInStoryBoard1.Begin();
        }
    }
} 