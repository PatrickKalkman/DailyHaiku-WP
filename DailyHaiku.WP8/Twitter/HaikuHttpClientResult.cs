using System;
using System.Threading;

namespace DailyHaiku.WP8.Twitter
{
    public class HaikuHttpClientResult : IAsyncResult
    {
        public string Response { get; set; }
        public string Error { get; set; }

        public bool IsCompleted { get; private set; }
        public WaitHandle AsyncWaitHandle { get; private set; }
        public object AsyncState { get; private set; }
        public bool CompletedSynchronously { get; private set; }
    }
}