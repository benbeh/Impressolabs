using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class BookmarkedJob
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public DateTime BookmarkedData { get; set; }
    }
}
