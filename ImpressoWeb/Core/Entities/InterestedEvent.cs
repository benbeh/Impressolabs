using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class InterestedEvent
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public DateTime BookmarkedData { get; set; }
    }
}
