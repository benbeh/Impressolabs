using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }


        public virtual ICollection<AppUserSkill> AppUserSkills { get; set; }

        public virtual ICollection<JobSkill> JobSkills { get; set; }
    }
}