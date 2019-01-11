using System;
using System.Collections.Generic;
using Core.Enum;

namespace BLL.ViewModels
{
    public class PersonProfileViewModel
    {
        public string Id { get; set; }
        public string Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string fullName { get; set; }
        public string UserName { get; set; }
        public string CompanyPosition { get; set; }
        public StatusEnum Status { get; set; }
        public string Location { get; set; }
        public int Connections { get; set; }
        public bool IsConnected { get; set; }
        public bool IsBookmarked { get; set; }
        public string Intro { get; set; }
        public DateTime LastUpdate { get; set; }
        //public string Education { get; set; }
        public string Passion { get; set; }
        public decimal? Salary { get; set; }
        public string PersonalityMatch { get; set; }        
        public JobTypeEnum JobType { get; set; }
        public string CV { get; set; }
        public ExperienceEnum Experience { get; set; }
        public IndustryEnum Industry { get; set; }
        public string EthereumAddress { get; set; }

        public List<string> PresentCompanies { get; set; } 
        public List<string> PastCompanies { get; set; }       
        public List<TestimonialViewModel> Testimonials { get; set; }
        public List<string> Accomplishments { get; set; }
        //public List<string> Skills { get; set; }
        public List<SkillViewModel> Skills { get; set; }
        public List<string> Certificates { get; set; }
        public List<string> Educations { get; set; }
    }
}
