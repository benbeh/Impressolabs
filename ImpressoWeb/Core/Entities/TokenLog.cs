using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class TokenLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }

        [ForeignKey(nameof(AppUser)), Required]
        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        [ForeignKey(nameof(AppUser)), Required]
        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }

        [Required]
        public int Count { get; set; }
        public string Message { get; set; }
    }
}
