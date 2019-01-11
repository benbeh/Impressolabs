using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateOfPost { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsNewest { get; set; }


        [ForeignKey(nameof(Entities.AppUser)), Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}