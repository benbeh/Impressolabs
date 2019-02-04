using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Enum;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string Photo { get; set; }

        public string CV { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AdditionalInformation { get; set; }

        public string EthereumAddress { get; set; }

        [Required]
        [DefaultValue(0)]
        public int Tokens { get; set; }

        public string Profession { get; set; }

        public string Location { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }

        public string CompanyPosition { get; set; }

        public string Intro { get; set; }

        //public string Education { get; set; }

        public string Passion { get; set; }

        public decimal? Salary { get; set; }

        public string PersonalityMatch { get; set; }


        [Required]
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

        public virtual int StatusId
        {
            get => (int)Status;
            set => Status = (StatusEnum)value;
        }
        [NotMapped]
        public StatusEnum Status { get; set; }


        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<AppUserJob> AppUserJobs { get; set; }

        public virtual ICollection<AppUserSkill> AppUserSkills { get; set; }

        public virtual ICollection<InterestedEvent> InterestedEvents { get; set; }

        public virtual ICollection<BookmarkedJob> BookmarkedJobs { get; set; }

        public virtual ICollection<ProjectAppUser> ProjectAppUsers { get; set; }

        public virtual ICollection<CompanyAppUser> CompanyAppUsers { get; set; }

        public virtual ICollection<AppUserCertificate> AppUserCertificates { get; set; }

        public virtual ICollection<AppUserEducation> AppUserEducations { get; set; }

        public virtual ICollection<BlockedUser> BlockedUsers { get; set; }
        public virtual ICollection<BlockedUser> BlockedByUsers { get; set; }

        public virtual ICollection<BookmarkedUser> BookmarkedUsers { get; set; }
        public virtual ICollection<BookmarkedUser> BookmarkedByUsers { get; set; }

        public virtual ICollection<ConnectedUser> ConnectedUsers { get; set; }
        public virtual ICollection<ConnectedUser> ConnectedByUsers { get; set; }

        public virtual ICollection<Testimonial> ReceivedTestimonials { get; set; }
        public virtual ICollection<Testimonial> SentTestimonials { get; set; }

        public virtual ICollection<Verificator> VerificatedTestimonials { get; set; }

        public virtual ICollection<Accomplishment> Accomplishments { get; set; }

        public virtual ICollection<TokenLog> SendedTokens { get; set; }
        public virtual ICollection<TokenLog> ReceivedTokens { get; set; }

        public AppUser()
        {
            Notifications = new List<Notification>();

            AppUserJobs = new List<AppUserJob>();

            AppUserSkills = new List<AppUserSkill>();

            InterestedEvents = new List<InterestedEvent>();

            BookmarkedJobs = new List<BookmarkedJob>();

            ProjectAppUsers = new List<ProjectAppUser>();

            CompanyAppUsers = new List<CompanyAppUser>();

            AppUserCertificates = new List<AppUserCertificate>();

            AppUserEducations = new List<AppUserEducation>();

            BlockedUsers = new List<BlockedUser>();
            BlockedByUsers = new List<BlockedUser>();

            BookmarkedUsers = new List<BookmarkedUser>();
            BookmarkedByUsers = new List<BookmarkedUser>();

            ConnectedUsers = new List<ConnectedUser>();
            ConnectedByUsers = new List<ConnectedUser>();

            ReceivedTestimonials = new List<Testimonial>();
            SentTestimonials = new List<Testimonial>();

            VerificatedTestimonials = new List<Verificator>();

            Accomplishments = new List<Accomplishment>();

            SendedTokens = new List<TokenLog>();
            ReceivedTokens = new List<TokenLog>();
    }
    }
}
