using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enum;

namespace Core.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; }

        public string Location { get; set; }

        public string Intro { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }


        public virtual int IndustryId
        {
            get => (int)Industry;
            set => Industry = (IndustryEnum)value;
        }
        [NotMapped]
        public IndustryEnum Industry { get; set; }


        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<CompanyAppUser> CompanyAppUsers { get; set; }

        public virtual ICollection<CompanyCertificate> CompanyCertificates { get; set; }
    }
}