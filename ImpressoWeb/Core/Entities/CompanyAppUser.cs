using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class CompanyAppUser
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsWorkingNow { get; set; }
    }
}