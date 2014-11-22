using System.Collections.Generic;

namespace DailyHaiku.WP8.Twitter
{
    public class RootObject
    {
        public List<Status> statuses { get; set; }
        public SearchMetadata search_metadata { get; set; }
    }
}