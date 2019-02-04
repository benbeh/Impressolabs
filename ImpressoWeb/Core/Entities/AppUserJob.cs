using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class AppUserJob
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public bool IsConnected { get; set; }
        [Required]
        public DateTime DateOfPost { get; set; }
    }
}