using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class BookmarkedUser
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string BookmarkedAppUserId { get; set; }
        public AppUser BookmarkedAppUser { get; set; }

        public DateTime BookmarkedData { get; set; }
    }
}
