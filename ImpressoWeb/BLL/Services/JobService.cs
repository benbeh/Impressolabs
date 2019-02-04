using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using DAL.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Enum;

namespace BLL.Services
{
    public class JobService : Service<Job, JobViewModel>, IJobService
    {
        public JobService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Jobs)
        {

        }

        
        public IEnumerable<JobViewModel> GetAllByName(string name)
        {
            return Mapping.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(Database.Jobs.GetAll()
                .Where(job => job.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public void CreateJob(CreateJobViewModel model)
        {
            Job createdJob = new Job()
            {
                Name = model.Title,
                Description = model.Description,
                ProjectId = model.ProjectId,
                DateOfPost = model.PostedTime,
                Location = model.Location,
                TypeOfWork = model.TypeOfWork,
                Experience = model.Level,
            };

            var skills = model.Skills;


            foreach (var skill in skills)
            {
                var skillDB = Database.Skills.GetAll().FirstOrDefault(s => s.Name == skill);

                if(skillDB == null)
                {
                    skillDB = Database.Skills.Add(new Skill { Name = skill, Description = null });
                }
                createdJob.JobSkills.Add(new JobSkill { Skill = skillDB, Job = createdJob });
            }

            Database.Jobs.Add(createdJob);

            Database.Save();
        }

        public IEnumerable<JobViewModel> GetListJobs(string userId)
        {
            var userSkills = Database.AppUserSkills.GetAll().Include(a => a.Skill).Where(p => p.AppUserId == userId).Select(s=>s.Skill.Name).ToList();
            
            var jobs = (Database.Jobs.GetAll()
                .Include(c => c.Project).ThenInclude(project => project.Company));

            var userInterviews = Database.AppUserJobs.GetAll().GroupBy(interview => interview.JobId);

            var joined = jobs.GroupJoin(userInterviews, job => job.Id, userInterview => userInterview.Key,
                (job, userInterview) => new JobViewModel
                {
                    Id = job.Id,
                    Title = job.Name,
                    IsBookmarked = Database.BookmarkedJobs.GetAll().Any(bookmarkedJob => bookmarkedJob.AppUserId == userId && bookmarkedJob.JobId == job.Id),
                    IsApplied = Database.AppUserJobs.GetAll().Any(appliedJob => appliedJob.AppUserId == userId && appliedJob.JobId == job.Id),
                    Location = job.Location,
                    Level = job.Experience,
                    CompanyName = job.Project.Company.Name,
                    CompanyId = job.Project.CompanyId,
                    CompanyLogoSource = job.Project.Company.Photo,
                    ApplicantsCount = userInterview.Count(),
                    PostedTime = job.DateOfPost,
                    Description = job.Description,
                    TypeOfWork = job.TypeOfWork,
                    Skills = job.JobSkills.Where(jobSkill => jobSkill.JobId == job.Id).Select(jobSKill => jobSKill.Skill.Name)
                }).ToList();


            foreach(var item in joined)
            {
                var jobSkills = item.Skills.ToList();
                List<string> duplicates = userSkills.Intersect(jobSkills).ToList();
                if(duplicates.Count >= 3)
                {
                    item.TopSkillsMatch = true;
                }
            }
            
            return joined;
        }

        public IList<string> GetTopSkillsMatch(string userId, int jobId)
        {
            var jobSkills = Database.JobSkills.GetAll().Include(jobskill => jobskill.Skill).Where(jobSkill => jobSkill.JobId == jobId).Select(skill => skill.Skill.Name).ToList();
            var userSkills = Database.AppUserSkills.GetAll().Include(userSkill => userSkill.Skill).Where(userSkill => userSkill.AppUserId == userId).Select(skill => skill.Skill.Name).ToList();
            List<string> duplicates = userSkills.Intersect(jobSkills).ToList();
            return duplicates;          
        }

        public JobViewModel GetJobInfo(string userId, int jobId)
        {
            var job = Database.Jobs.GetAll().Include(jobs => jobs.Project).ThenInclude(project => project.Company).Include(jobs => jobs.JobSkills).ThenInclude(jobs=>jobs.Skill).FirstOrDefault(jobs => jobs.Id == jobId);

            var jobViewModel = Mapping.Map<Job, JobViewModel>(job);
            jobViewModel.IsApplied = Database.AppUserJobs.GetAll().Any(appliedJob => appliedJob.AppUserId == userId && appliedJob.JobId == job.Id);
            return jobViewModel;
        }

        public JobDetailsViewModel GetJobDetailsViewModel(int jobId)
        {
            var result = Database.Jobs.GetAll().AsNoTracking().Include(job => job.JobSkills).ThenInclude(jobSkill => jobSkill.Skill).Include(job => job.JobCertificates).ThenInclude(jobCertificate => jobCertificate.Certificate).Include(job => job.AppUserJobs).ThenInclude(appUserJob => appUserJob.AppUser).FirstOrDefault(jobs => jobs.Id == jobId);
            return Mapping.Map<Job, JobDetailsViewModel>(result);
        }

        public void AddUser(string userId, int jobId)
        {
            var job = Database.AppUserJobs.GetAll().AsNoTracking().FirstOrDefault(appUserJob => appUserJob.AppUserId == userId && appUserJob.JobId == jobId);
            if (job != null)
            {
                job.IsConnected = true;
                Database.AppUserJobs.Update(job);
            }
            else
            {
                Database.AppUserJobs.Add(new AppUserJob() { AppUserId = userId, JobId = jobId, IsConnected = true });
            }
            Database.Save();
        }

        public object GetFilters()
        {
            return new
            {
                Companies = Database.Companies.GetAll().Select(company => company.Name),
                JobTypes = EnumHelper.GetDescriptions(typeof(JobTypeEnum)),
                Skills = Database.Skills.GetAll().Select(skill => skill.Name),
                Experience = EnumHelper.GetDescriptions(typeof(ExperienceEnum)),
                Industries = EnumHelper.GetDescriptions(typeof(IndustryEnum)),
                Certificates = Database.Certificates.GetAll().Select(certificate => certificate.Name),
                Locations = GetLocations()
            };
        }

        public IEnumerable<JobViewModel> Filter(JobFilterViewModel filter)
        {
            IEnumerable<Job> result = Database.Jobs.GetAll().Include(c => c.Project).ThenInclude(project => project.Company).ThenInclude(company => company.CompanyAppUsers).Include(job => job.JobSkills).ThenInclude(jobSkill => jobSkill.Skill);

            // sort by conformity
            if (filter.IsMostRelevant && filter.Skills != null)
                result = SortByMostRelevant(result.ToList(), filter.Skills).ToList();
            else
                result = SortByMostRecent(result);

            // sort by name
            if (!string.IsNullOrEmpty(filter.CompanyName))
            {
                result = result.Where(job => job.Project.Company.Name.Trim().ToLowerInvariant() == filter.CompanyName.Trim().ToLowerInvariant());
            }

            // sort by location
            if (!string.IsNullOrEmpty(filter.Location))
            {
                result = result.Where(job => !String.IsNullOrEmpty(job.Location) && job.Location.Trim().ToLowerInvariant().Contains(filter.Location.Trim().ToLowerInvariant()));
            }

            // sort by job types
            if (filter.JobTypes != null && filter.JobTypes.Count() != 0)
            {
                result = result.Where(job => filter.JobTypes.Contains(job.JobType.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by skills
            if (filter.Skills != null && filter.Skills.Count() != 0)
            {
                result = result.Where(job => job.JobSkills.Any(jobSkill => jobSkill.Job == job && filter.Skills.Any(skill => skill.Contains(jobSkill.Skill.Name, StringComparison.InvariantCultureIgnoreCase))));
            }

            // sort by experience
            if (filter.Experience != null && filter.Experience.Count() != 0)
            {
                result = result.Where(job => filter.Experience.Contains(job.Experience.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by industry
            if (!string.IsNullOrEmpty(filter.Industry))
            {
                result = result.Where(job => job.Industry.GetDescription().Trim().ToLowerInvariant() == filter.Industry.Trim().ToLowerInvariant());
            }

            // sort by certificates
            if (filter.Certificates != null && filter.Certificates.Count() != 0)
            {
                //result = result.Where(job => job.Certificates.Any(jobCertificate => jobCertificate.Job == job && filter.Certificates.Contains(jobCertificate.Certificate.Name, StringComparer.InvariantCultureIgnoreCase)));
            }

            // sort by company size
            if (filter.MinCompanySize != filter.MaxCompanySize)
            {
                result = result.Where(job =>
                job.Project.Company.CompanyAppUsers.Count(companyAppUser => companyAppUser.CompanyId == job.Project.Company.Id) > filter.MinCompanySize &&
                job.Project.Company.CompanyAppUsers.Count(companyAppUser => companyAppUser.CompanyId == job.Project.Company.Id) < filter.MaxCompanySize);
            }

            return Mapping.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(result);
        }

        public IEnumerable<Job> SortByMostRelevant(IEnumerable<Job> jobs, List<string> skills)
        {
            return jobs.OrderByDescending(job => job.JobSkills.Count(jobSKill => jobSKill.JobId == job.Id &&
                skills.Contains(jobSKill.Skill.Name, StringComparer.InvariantCultureIgnoreCase)));
        }

        public IEnumerable<Job> SortByMostRecent(IEnumerable<Job> jobs)
        {
            return jobs.OrderByDescending(job => job.DateOfPost);
        }

        public IEnumerable<TagViewModel> GetBookmakedJobsByUserId(string userId)
        {
            var bookmarkedJobs = Database.BookmarkedJobs.GetAll().Where(bookmarkedJob => bookmarkedJob.AppUserId == userId).Include(bookmarkedJob => bookmarkedJob.Job).Select(tag => new TagViewModel { Name = tag.Job.Name, Type = "Job", Area = tag.Job.Industry.GetDescription(),LogoSource = tag.Job.Project.Company.Photo, BookmarkedData = tag.BookmarkedData }).ToList();
            return bookmarkedJobs;
        }


        public bool ApplyForProjectByUserId(AppUserJobViewModel appUserJobViewModel)
        {
            if (Database.AppUserJobs.GetAll().Any(appliedJob => appliedJob.AppUserId == appUserJobViewModel.AppUserId && appliedJob.JobId == appUserJobViewModel.JobId))
            {
                return false;
            }
            else
            {
                Database.AppUserJobs.Add(Mapping.Map<AppUserJobViewModel, AppUserJob>(appUserJobViewModel));
                Database.Save();
                return true;
            }

        }

        public IEnumerable<JobViewModel> ListAppliedJobsOfCurrentUser(string userId)
        {
            var jobs = Database.AppUserJobs.GetAll().Where(userJob => userJob.AppUserId == userId).Include(userJob => userJob.Job).ThenInclude(job => job.Project).ThenInclude(project => project.Company ).Select(jobViewModel => new JobViewModel { Id = jobViewModel.JobId, Title = jobViewModel.Job.Title, ProjectName = jobViewModel.Job.Project.Name, PostedTime = jobViewModel.Job.DateOfPost, PositionsCount = 3, CompanyId = jobViewModel.Job.Project.CompanyId }).ToList();

            return jobs;
        }

        public void Update(JobDetailsViewModel jobViewModel)
        {
            if (jobViewModel == null)
                throw new Exception(typeof(JobDetailsViewModel) + " wasn't set");

            var job = Mapping.Map<JobDetailsViewModel, Job>(jobViewModel);

            // delete skills
            Database.JobSkills.Delete(Database.JobSkills.GetAll().AsNoTracking().Where(jobSkill => jobSkill.JobId == jobViewModel.Id));
            // add skills
            if (jobViewModel.Skills != null && jobViewModel.Skills.Count() != 0)
            {
                foreach (var skillId in jobViewModel.Skills.Distinct())
                {
                    Database.JobSkills.Add(new JobSkill()
                    {
                        JobId = jobViewModel.Id,
                        SkillId = Convert.ToInt32(skillId)
                    });
                }
            }
            
            // delete certificates
            Database.JobCertificates.Delete(Database.JobCertificates.GetAll().AsNoTracking().Where(jobCertificate => jobCertificate.JobId == jobViewModel.Id));
            // add certificates
            if (jobViewModel.JobCertificates != null && jobViewModel.JobCertificates.Count() != 0)
            {
                foreach (var certificateId in jobViewModel.JobCertificates.Distinct())
                {
                    Database.JobCertificates.Add(new JobCertificate()
                    {
                        JobId = jobViewModel.Id,
                        CertificateId = Convert.ToInt32(certificateId)
                    });
                }
            }

            job.JobSkills.Clear();
            job.JobCertificates.Clear();
            Database.Jobs.Update(job);
            Database.Save();
        }

        public List<string> GetLocations()
        {
            return Database.Jobs.GetAll().Where(job => !String.IsNullOrEmpty(job.Location)).Select(job => job.Location).Distinct().ToList();
        }
    }
}