using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DailyHaiku.WP8.Views
{
    public static class ImageProperties
    {
        public static Dictionary<Uri, BitmapImage> imageCache = new Dictionary<Uri, BitmapImage>();

        public static readonly DependencyProperty SourceWithCustomRefererProperty =
            DependencyProperty.RegisterAttached(
                "SourceWithCustomReferer",
                typeof(Uri),
                typeof(ImageProperties),
                new PropertyMetadata(OnSourceWithCustomRefererChanged));

        private static void OnSourceWithCustomRefererChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var image = (Image)o;
            var uri = (Uri)e.NewValue;

            if (DesignerProperties.IsInDesignTool)
            {
                // for the design surface we just load the image straight up
                image.Source = new BitmapImage(uri);
            }
            else
            {
                if (imageCache.ContainsKey(uri))
                {
                    image.Source = imageCache[uri];
                    return;
                }

                image.Source = null;

                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.Headers["Referer"] = uri.Host; // or your custom referer string here
                request.BeginGetResponse((result) =>
                                             {
                                                 try
                                                 {
                                                     Stream imageStream = request.EndGetResponse(result).GetResponseStream();
                                                     Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                                                                   {
                                                                                                       BitmapImage bitmapImage = new BitmapImage();
                                                                                                       bitmapImage.CreateOptions = BitmapCreateOptions.BackgroundCreation;
                                                                                                       bitmapImage.SetSource(imageStream);
                                                                                                       image.Source = bitmapImage;
                                                                                                       imageCache.Add(uri, bitmapImage);
                                                                                                   });
                                                 }
                                                 catch (WebException)
                                                 {
                                                     // add error handling
                                                 }
                                             }, null);
            }
        }

        public static Uri GetSourceWithCustomReferer(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image");
            }
            return (Uri)image.GetValue(SourceWithCustomRefererProperty);
        }

        public static void SetSourceWithCustomReferer(Image image, Uri value)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image");
            }
            image.SetValue(SourceWithCustomRefererProperty, value);
        }
    }
}