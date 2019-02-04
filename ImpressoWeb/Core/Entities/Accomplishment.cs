using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Accomplishment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }


        [ForeignKey(nameof(Entities.AppUser)), Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
