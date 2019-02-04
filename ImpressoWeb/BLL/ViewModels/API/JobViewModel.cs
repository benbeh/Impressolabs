using Core.Entities;
using Core.Enum;
using System;
using System.Collections.Generic;

namespace BLL.ViewModels.API
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime PostedTime { get; set; }
        public IndustryEnum Industry { get; set; }
        public JobTypeEnum JobType { get; set; }   
        public ExperienceEnum Level { get; set; }
        public TypeOfWorkEnum TypeOfWork { get; set; }


        public bool TopSkillsMatch { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsApplied { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyLogoSource { get; set; }
        public int ApplicantsCount { get; set; }
        public int PositionsCount { get; set; }

        public IEnumerable<string> Skills { get; set; }
        public List<AppUserJobViewModel> AppUserJobs { get; set; }
    }
}