using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Enum;

namespace Core.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string PictureSource { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }


        [Required]
        public virtual int PrivacyTypeId
        {
            get => (int)PrivacyType;
            set => PrivacyType = (PrivacyTypeEnum)value;
        }
        [NotMapped]
        public PrivacyTypeEnum PrivacyType { get; set; }


        [ForeignKey(nameof(Entities.Company)), Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        

        public virtual ICollection<InterestedEvent> InterestedEvents { get; set; }
    }
}
