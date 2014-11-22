using System.Collections.Generic;
using DailyHaiku.WP8.ViewModels;

namespace DailyHaiku.WP8.Twitter
{
    public class PreviousHaikusReceivedEvent
    {
        public List<Haiku> Haikus { get; set; }
    }
}