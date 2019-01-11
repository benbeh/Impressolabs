using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }


        [ForeignKey(nameof(Entities.Company)), Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; }


        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<ProjectAppUser> ProjectAppUsers { get; set; }
    }
}