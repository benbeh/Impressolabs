using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Verificator
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Entities.Testimonial)), Required]
        public int TestimonialId { get; set; }
        public Testimonial Testimonial { get; set; }

        [ForeignKey(nameof(Entities.AppUser)), Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}