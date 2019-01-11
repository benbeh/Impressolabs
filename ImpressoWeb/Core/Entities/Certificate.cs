using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public virtual ICollection<CompanyCertificate> CompanyCertificates { get; set; }
        public virtual ICollection<AppUserCertificate> AppUserCertificates { get; set; }
        public virtual ICollection<JobCertificate> JobCertificates { get; set; }

    }
}