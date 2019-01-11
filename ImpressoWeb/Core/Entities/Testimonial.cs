using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Core.Entities
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        public string RecommenderId { get; set; }
        public AppUser Recommender { get; set; }

        public string RecommendedUserId { get; set; }
        public AppUser RecommendedUser { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DateOfPost { get; set; }


        public virtual ICollection<Verificator> Verificators { get; set; }
    }
}
