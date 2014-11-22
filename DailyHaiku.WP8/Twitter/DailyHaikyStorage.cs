using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using DailyHaiku.WP8.ViewModels;
using IsolatedStorageExtensions;
using Newtonsoft.Json;

namespace DailyHaiku.WP8.Twitter
{
    public class DailyHaikyStorage
    {
        private readonly TwitterDateTimeParser parser;
        private const string DailyHaikyStorageFile = "DailyHaiku.json";
        private const string PreviousDailyHaikyStorageFile = "PreviousDailyHaiku.json";

        private HaikuReceivedEvent currentDailyHaiku;

        public DailyHaikyStorage(TwitterDateTimeParser parser)
        {
            this.parser = parser;
        }

        public bool IsAlreadyAvailable()
        {
            if (IsolatedStorageHelper.FileExists(DailyHaikyStorageFile))
            {
                HaikuReceivedEvent storedDailyHaiku = GetStoredHaiku();
                DateTime storedHaikuDateTime = parser.ParseDateTime(storedDailyHaiku.Haiku.CreatedAt);
                if (storedHaikuDateTime.Date.Equals(DateTime.Now.Date))
                {
                    //A stored haiku is available and is recent.
                    currentDailyHaiku = storedDailyHaiku;
                    return true;
                }

                //A stored haiku is available but is not from today.
                return false;
            }

            //No stored haiku is available.
            return false;
        }

        private HaikuReceivedEvent GetStoredHaiku()
        {
            if (IsolatedStorageHelper.FileExists(DailyHaikyStorageFile))
            {
                string dailyHaikuJson = IsolatedStorageHelper.ReadFileText(DailyHaikyStorageFile);
                return JsonConvert.DeserializeObject<HaikuReceivedEvent>(dailyHaikuJson);
            }
            return null;
        }

        public HaikuReceivedEvent GetCurrentHaiku()
        {
            return currentDailyHaiku;
        }

        public void StoreTodaysHaiku(HaikuReceivedEvent haikuEvent)
        {
            string serializedDailyHaiku = JsonConvert.SerializeObject(haikuEvent);
            IsolatedStorageHelper.MakeFile(serializedDailyHaiku, DailyHaikyStorageFile);
        }

        public void AddToPreviousHaikus(HaikuReceivedEvent haikuEvent)
        {
            List<Haiku> previousHaikus;

            if (!IsolatedStorageHelper.FileExists(PreviousDailyHaikyStorageFile))
            {
                previousHaikus = JsonConvert.DeserializeObject<Haiku[]>(ReadInitialPreviousFromXap()).ToList();
            }
            else
            {
                string previousHaikusJson = IsolatedStorageHelper.ReadFileText(PreviousDailyHaikyStorageFile);
                previousHaikus = JsonConvert.DeserializeObject<List<Haiku>>(previousHaikusJson);
            }

            previousHaikus.Add(haikuEvent.Haiku);
            string serializedPreviousHaikus = JsonConvert.SerializeObject(previousHaikus);
            IsolatedStorageHelper.MakeFile(serializedPreviousHaikus, PreviousDailyHaikyStorageFile);
        }

        public Haiku[] GetPreviousHaikus()
        {
            string previousHaikusJson = IsolatedStorageHelper.ReadFileText(PreviousDailyHaikyStorageFile);
            if (string.IsNullOrEmpty(previousHaikusJson))
            {
                previousHaikusJson = ReadInitialPreviousFromXap();
            }
            return JsonConvert.DeserializeObject<List<Haiku>>(previousHaikusJson).ToArray();
        }

        private string ReadInitialPreviousFromXap()
        {
            StreamResourceInfo streamResourceInfo = Application.GetResourceStream(new Uri("Assets/InitialPreviousHaikus.json", UriKind.Relative));

            if (streamResourceInfo != null)
            {
                using (Stream resourceStream = streamResourceInfo.Stream)
                {
                    if (resourceStream.CanRead)
                    {
                        using (var streamReader = new StreamReader(resourceStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
            throw new InvalidOperationException("Cannot find or read the message file.");
        }
    }
}