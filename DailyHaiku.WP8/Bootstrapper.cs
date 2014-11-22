using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using DailyHaiku.WP8.Assets;
using DailyHaiku.WP8.Common;
using DailyHaiku.WP8.Model;
using DailyHaiku.WP8.Twitter;
using DailyHaiku.WP8.ViewModels;
using Microsoft.Phone.Controls;

namespace DailyHaiku.WP8
{
    public class Bootstrapper : PhoneBootstrapper
    {
        private PhoneContainer container;

        private LocalyticsSession appSession;

        public Bootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        protected override void Configure()
        {
            container = new PhoneContainer(RootFrame);

            container.RegisterPhoneServices();
            container.PerRequest<MainPageViewModel>();
            container.PerRequest<DailyHaikuRetriever>();
            container.PerRequest<DailyHaikuTwitterQueryCreator>();
            container.PerRequest<HaikuHttpClient>();
            container.PerRequest<HaikuViewModel>();
            container.PerRequest<TimeBetweenCalculator>();
            container.PerRequest<PreviousHaikusRetriever>();
            container.PerRequest<BackgroundImageBrush>();
            container.PerRequest<PreviousHaikusViewModel>();
            container.PerRequest<TermExtractor>();
            container.PerRequest<FiveHundredPixRetriever>();
            container.PerRequest<FiveHundredPixQueryCreator>();
            container.PerRequest<TwitterDateTimeParser>();
            container.PerRequest<DailyHaikyStorage>();
            container.PerRequest<PrivacyViewModel>();
            container.PerRequest<Share>();
            container.PerRequest<HaikuExtractor>();

            AddCustomConventions();
        }

        private void OpenLocalytics()
        {
            appSession = new LocalyticsSession("hfgc765765765-4a650a24-bbc5-11e2-0dff-004a77f8b47f");
            appSession.open();
            appSession.upload();
        }

        protected override void OnActivate(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            OpenLocalytics();
            base.OnActivate(sender, e);
        }

        protected override void OnLaunch(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            OpenLocalytics();
            UpdateReviewCounter();
            base.OnLaunch(sender, e);
        }

        private static void UpdateReviewCounter()
        {
            IsolatedStorageSettings.ApplicationSettings["askforreview"] = false;

            int started = 0;
            if (IsolatedStorageSettings.ApplicationSettings.Contains("started"))
            {
                started = (int)IsolatedStorageSettings.ApplicationSettings["started"];
            }
            started++;
            IsolatedStorageSettings.ApplicationSettings["started"] = started;
            if (started == 3)
            {
                IsolatedStorageSettings.ApplicationSettings["askforreview"] = true;
            }
        }

        protected override void OnDeactivate(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            appSession.close();
            base.OnDeactivate(sender, e);
        }

        protected override void OnClose(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            appSession.close();
            base.OnClose(sender, e);
        }

        protected override void OnUnhandledException(object sender, System.Windows.ApplicationUnhandledExceptionEventArgs e)
        {
            Dictionary<String, String> attributes = new Dictionary<string, string>();
            attributes.Add("exception", e.ExceptionObject.Message);
            appSession.tagEvent("App crash", attributes);
            base.OnUnhandledException(sender, e);
        }

        static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<BindableAppBarButton>(
                Control.IsEnabledProperty, "DataContext", "Click");
            ConventionManager.AddElementConvention<BindableAppBarMenuItem>(
                Control.IsEnabledProperty, "DataContext", "Click");
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            return new TransitionFrame();
        }
    }
}
