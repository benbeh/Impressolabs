using Core.Enum;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BLL.ViewModels
{
    public class JobDetailsViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
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

        public IFormFile PhotoFile { get; set; }

        public int ApplicantsCount {
            get
            {
                if (AppUsers == null || AppUsers.Count == 0)
                    return 0;
                return AppUsers.Count;
            }
        }


        public JobTypeEnum JobType { get; set; }
        public ExperienceEnum Experience { get; set; }

        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<string> JobCertificates { get; set; }
        public List<AppUserViewModel> AppUsers { get; set; }
    }
}
