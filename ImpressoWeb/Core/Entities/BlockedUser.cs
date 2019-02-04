using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class BlockedUser
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string BlockedAppUserId { get; set; }
        public AppUser BlockedAppUser { get; set; }
    }
}
