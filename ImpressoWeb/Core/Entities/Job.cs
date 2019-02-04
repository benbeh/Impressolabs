using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enum;

namespace Core.Entities
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Responsibility { get; set; }

        public decimal? Salary { get; set; }

        public string PersonalityMatch { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Photo { get; set; }

        [Required]
        public DateTime DateOfPost { get; set; }


        public virtual int IndustryId
        {
            get => (int)Industry;
            set => Industry = (IndustryEnum)value;
        }
        [NotMapped]
        public IndustryEnum Industry { get; set; }

        public virtual int JobTypeId
        {
            get => (int)JobType;
            set => JobType = (JobTypeEnum)value;
        }
        [NotMapped]
        public JobTypeEnum JobType { get; set; }

        public virtual int ExperienceId
        {
            get => (int)Experience;
            set => Experience = (ExperienceEnum)value;
        }
        [NotMapped]
        public ExperienceEnum Experience { get; set; }

        public virtual int TypeOfWorkId
        {
            get => (int)TypeOfWork;
            set => TypeOfWork = (TypeOfWorkEnum)value;
        }
        [NotMapped]
        public TypeOfWorkEnum TypeOfWork { get; set; }


        [ForeignKey(nameof(Entities.Project)), Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }


        public virtual ICollection<AppUserJob> AppUserJobs { get; set; }

        public virtual ICollection<JobSkill> JobSkills { get; set; }

        public virtual ICollection<BookmarkedJob> BookmarkedJobs { get; set; }

        public virtual ICollection<JobCertificate> JobCertificates { get; set; }

        public Job()
        {
            JobSkills = new List<JobSkill>();
            AppUserJobs = new List<AppUserJob>();
        }
    }
}