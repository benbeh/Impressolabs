using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using DAL.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public IUnitOfWork Database { get; set; }

        public AppUserService(UserManager<AppUser> userManager, IHostingEnvironment environment, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _hostingEnvironment = environment;
            Database = unitOfWork;
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _userManager.Users;
        }

        public AppUser Get(string id)
        {
            if (id == null)
                throw new Exception("Id wasn't set");
            var item = _userManager.FindByIdAsync(id).Result;
            if (item == null)
                throw new Exception("AppUser wasn't found");
            return item;
        }

        public CompanyProfileViewModel GetCompanyProfile(string userId)
        {
            var user = _userManager.Users
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).ThenInclude(company => company.Projects).ThenInclude(project => project.Jobs).ThenInclude(job => job.AppUserJobs)
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).ThenInclude(company => company.Projects).ThenInclude(project => project.ProjectAppUsers)
                .Include(appUser => appUser.ReceivedTokens).ThenInclude(receivedToken => receivedToken.Sender)
                .FirstOrDefault(appUser => appUser.Id == userId);
            if (user == null)
                throw new Exception("User wasn't found");
            var firstCompanyAppUser = user.CompanyAppUsers.FirstOrDefault();
            if (firstCompanyAppUser == null)
                return null;
            return Mapping.Map<Company, CompanyProfileViewModel>(firstCompanyAppUser.Company, opt => opt.Items["CurrentUserId"] = userId);
        }

        public ConnectPersonCompanyViewModel GetConnectPersonCompanyViewModel(string userId)
        {
            var user = _userManager.Users
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).ThenInclude(company => company.Projects).ThenInclude(project => project.Jobs).ThenInclude(job => job.AppUserJobs)
                .FirstOrDefault(appUser => appUser.Id == userId);
            if (user == null)
                throw new Exception("User wasn't found");
            var firstCompanyAppUser = user.CompanyAppUsers.FirstOrDefault();
            if (firstCompanyAppUser == null)
                return null;

            var companyViewModel = Mapping.Map<Company, ConnectPersonCompanyViewModel>(firstCompanyAppUser.Company);

            // use only connected by company
            foreach (var job in companyViewModel.Jobs)
            {
                job.AppUserJobs = job.AppUserJobs.Where(appUserJob => appUserJob.IsConnected).ToList();
            }
            return companyViewModel;
        }

        public SavePersonForProjectViewModel GetSavePersonForProjectViewModel(string userId)
        {
            var user = _userManager.Users
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).ThenInclude(company => company.Projects).ThenInclude(project => project.ProjectAppUsers)
                .FirstOrDefault(appUser => appUser.Id == userId);
            if (user == null)
                throw new Exception("User wasn't found");
            var firstCompanyAppUser = user.CompanyAppUsers.FirstOrDefault();
            if (firstCompanyAppUser == null)
                return null;
            return Mapping.Map<Company, SavePersonForProjectViewModel>(firstCompanyAppUser.Company);
        }

        public List<JobViewModel> GetCurrentCompanyJobsThatNotConnectedWithUser(string userId, string currentUserId)
        {
            var user = _userManager.Users
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).ThenInclude(company => company.Projects).ThenInclude(project => project.Jobs).ThenInclude(job => job.AppUserJobs)
                .FirstOrDefault(appUser => appUser.Id == currentUserId);
            if (user == null)
                throw new Exception("User wasn't found");
            var firstCompanyAppUser = user.CompanyAppUsers.FirstOrDefault();
            if (firstCompanyAppUser == null)
                return null;
            return Mapping.Map<List<Job>, List<JobViewModel>>(firstCompanyAppUser.Company.
                Projects.SelectMany(project => project.Jobs).Where(job => job.AppUserJobs.All(appUserJob => appUserJob.AppUserId != userId || appUserJob.IsConnected == false)).ToList());
        }

        public List<ProjectViewModel> GetCurrentCompanyProjectsThatNotSavedWithUser(string userId, string currentUserId)
        {
            var user = _userManager.Users
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).ThenInclude(company => company.Projects).ThenInclude(project => project.ProjectAppUsers)
                .FirstOrDefault(appUser => appUser.Id == currentUserId);
            if (user == null)
                throw new Exception("User wasn't found");
            var firstCompanyAppUser = user.CompanyAppUsers.FirstOrDefault();
            if (firstCompanyAppUser == null)
                return null;
            return Mapping.Map<List<Project>, List<ProjectViewModel>>(firstCompanyAppUser.Company.
                Projects.Where(project => project.ProjectAppUsers.All(projectAppUser => projectAppUser.AppUserId != userId)).ToList());
        }

        public void ChangeCompanyImage(string userId, IFormFile image)
        {
            var user = _userManager.Users
                .Include(appUser => appUser.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).FirstOrDefault(appUser => appUser.Id == userId);
            if (user == null)
                throw new Exception("User wasn't found");
            var firstCompanyAppUser = user.CompanyAppUsers.FirstOrDefault();
            if (firstCompanyAppUser == null)
                throw new Exception("Company wasn't found");

            // save image to folder
            string filePath = "";
            if (image != null && image.Length > 0)
            {
                filePath = "/images/Companies/" + firstCompanyAppUser.Company.Id + ".png";

                using (var stream = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
            }

            // save to database
            firstCompanyAppUser.Company.Photo = filePath;
            Database.CompanyAppUsers.Update(firstCompanyAppUser);
            Database.Save();
        }

        public void VerifyTestimonial(string userId, int testimonialId)
        {
            Database.Verificators.Add(new Verificator() { TestimonialId = testimonialId, AppUserId = userId });
            Database.Save();
        }

        public PersonProfileViewModel GetPersonProfile(string id, string currentUserId)
        {

            /*if (id == null)
                throw new Exception("Id wasn't set");
            var item = _userManager.Users.Where(user => user.Id == id).Include(user => user.CompanyAppUsers)
                .ThenInclude(companyAppUser => companyAppUser.Company)
                    .Include(appUser => appUser.Accomplishments)
                    .Include(appUser => appUser.AppUserSkills)
                .ThenInclude(appUserSkill => appUserSkill.Skill).Include(appUser => appUser.AppUserCertificates)
                .ThenInclude(appUserCertificate => appUserCertificate.Certificate).Include(user => user.ReceivedTestimonials)
                .ThenInclude(testimonial => testimonial.Verificators)
                .ThenInclude(verificator => verificator.AppUser).Include(user => user.ReceivedTestimonials)
                .ThenInclude(testimonial => testimonial.Recommender)
                .ThenInclude(recommender => recommender.CompanyAppUsers)
                .ThenInclude(companyAppUser => companyAppUser.Company).First();
            if (item == null)
                throw new Exception("AppUser wasn't found");
            return Mapping.Map<AppUser, PersonProfileViewModel>(item, opt => opt.Items["CurrentUserId"] = currentUserId);
        */
            if (id == null)
                throw new Exception("Id wasn't set");

            var item = _userManager.Users.Where(user => user.Id == id).Include(user => user.CompanyAppUsers)
                .ThenInclude(companyAppUser => companyAppUser.Company)
                .Include(appUser => appUser.AppUserSkills)
                .ThenInclude(appUserSkill => appUserSkill.Skill)
                .Include(appUser => appUser.AppUserCertificates)
                .ThenInclude(appUserCertificate => appUserCertificate.Certificate)
                .Include(appUser => appUser.AppUserEducations)
                .ThenInclude(appUserEducation => appUserEducation.Education)
                .Include(user => user.ReceivedTestimonials)
                .ThenInclude(testimonial => testimonial.Verificators)
                .ThenInclude(verificator => verificator.AppUser).Include(user => user.ReceivedTestimonials)
                .ThenInclude(testimonial => testimonial.Recommender)
                .ThenInclude(recommender => recommender.CompanyAppUsers)
                .ThenInclude(companyAppUser => companyAppUser.Company).Include(user => user.Accomplishments)
                .Include(user => user.ConnectedByUsers).Include(user => user.ConnectedUsers).Include(user => user.BookmarkedByUsers).First();

            if (item == null)
                throw new Exception("AppUser wasn't found");

            var result = Mapping.Map<AppUser, PersonProfileViewModel>(item, opt => opt.Items["CurrentUserId"] = currentUserId);
            
            //count Connection of User
            var connectionCount = 0;
            foreach(var connection in Database.ConnectedUsers.GetAll())
            {
                if (connection.ConnectedAppUserId == item.Id)
                    connectionCount++;
            }
            result.Connections = connectionCount;


            return result;
        }

        public void Delete(string id)
        {
            if (id == null)
                throw new Exception("Id wasn't found");

            _userManager.DeleteAsync(_userManager.FindByIdAsync(id).Result);
        }

        // filter
        public PersonFilterValuesViewModel GetFilters()
        {
            return new PersonFilterValuesViewModel
            {
                Industries = EnumHelper.GetDescriptions(typeof(IndustryEnum)),
                JobTypes = EnumHelper.GetDescriptions(typeof(JobTypeEnum)),
                Skills = Database.Skills.GetAll().Select(skill => skill.Name).ToList(),
                Experience = EnumHelper.GetDescriptions(typeof(ExperienceEnum)),
                Certificates = Database.Certificates.GetAll().Select(certificate => certificate.Name).ToList(),
                CompanyNames = Database.Companies.GetAll().Select(company => company.Name).ToList()
            };
        }

        public IEnumerable<AppUser> Filter(IEnumerable<AppUser> result, PersonFilterValuesViewModel filter)
        {
            // sort by location
            if (!string.IsNullOrEmpty(filter.Location))
            {
                result = result.Where(user => user.Location != null && user.Location.Trim().Contains(filter.Location.Trim(), StringComparison.InvariantCultureIgnoreCase));
            }

            // sort by industry
            if (filter.Industries != null && filter.Industries.Count() != 0)
            {
                result = result.Where(user => filter.Industries.Contains(user.Industry.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by jobTypes
            if (filter.JobTypes != null && filter.JobTypes.Count() != 0)
            {
                result = result.Where(user => filter.JobTypes.Contains(user.JobType.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by skills
            if (filter.Skills != null && filter.Skills.Count() != 0)
            {
                result = result.Where(user => user.AppUserSkills.Any(appUserSkill => filter.Skills.Any(skill => skill.Equals(appUserSkill.Skill.Name, StringComparison.InvariantCultureIgnoreCase))));
            }

            // sort by experience
            if (filter.Experience != null && filter.Experience.Count() != 0)
            {
                result = result.Where(user => filter.Experience.Contains(user.Experience.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by certificates
            if (filter.Certificates != null && filter.Certificates.Count() != 0)
            {
                result = result.Where(user => user.AppUserCertificates.Any(appUserCertificates => filter.Certificates.Any(certificate => certificate.Equals(appUserCertificates.Certificate.Name, StringComparison.InvariantCultureIgnoreCase))));
            }

            // sort by company names
            if (filter.CompanyNames != null && filter.CompanyNames.Count() != 0)
            {
                result = result.Where(user => user.CompanyAppUsers.Any(companyAppUser => filter.CompanyNames.Any(companyName => companyName.Equals(companyAppUser.Company.Name, StringComparison.InvariantCultureIgnoreCase))));
            }

            return result;
        }

        // skills
        public void AddSkill(AppUserSkillViewModel appUserSkill)
        {
            Database.AppUserSkills.Add(Mapping.Map<AppUserSkillViewModel, AppUserSkill>(appUserSkill));
            Database.Save();
        }

        // blockedUsers
        public void BlockUser(BlockedUserViewModel blockedUser)
        {
            Database.BlockedUsers.Add(Mapping.Map<BlockedUserViewModel, BlockedUser>(blockedUser));
            Database.Save();
        }

        public void UnblockUser(BlockedUserViewModel blockedUser)
        {
            Database.BlockedUsers.Delete(Mapping.Map<BlockedUserViewModel, BlockedUser>(blockedUser));
            Database.Save();
        }

        // bookmarkedUsers
        public void BookmarkedUser(BookmarkedUserViewModel bookmarkedUser)
        {
            Database.BookmarkedUsers.Add(Mapping.Map<BookmarkedUserViewModel, BookmarkedUser>(bookmarkedUser));
            Database.Save();

        }

        public void UnBookmarkedUser(BookmarkedUserViewModel bookmarkedUser)
        {
            Database.BookmarkedUsers.Delete(Mapping.Map<BookmarkedUserViewModel, BookmarkedUser>(bookmarkedUser));
            Database.Save();
        }

        // bookmarkedJobs
        public void BookmarkedJob(BookmarkedJobViewModel bookmarkedJob)
        {
            Database.BookmarkedJobs.Add(Mapping.Map<BookmarkedJobViewModel, BookmarkedJob>(bookmarkedJob));
            Database.Save();
        }

        public void UnBookmarkedJob(BookmarkedJobViewModel bookmarkedJob)
        {
            Database.BookmarkedJobs.Delete(Mapping.Map<BookmarkedJobViewModel, BookmarkedJob>(bookmarkedJob));
            Database.Save();
        }

        // interestedEvent
        public void InterestedEvent(InterestedEventViewModel interestedEvent)
        {
            Database.InterestedEvents.Add(Mapping.Map<InterestedEventViewModel, InterestedEvent>(interestedEvent));
            Database.Save();
        }

        public void UnInterestedEvent(InterestedEventViewModel interestedEvent)
        {
            Database.InterestedEvents.Delete(Mapping.Map<InterestedEventViewModel, InterestedEvent>(interestedEvent));
            Database.Save();
        }

        public IEnumerable<TagViewModel> GetBookmakedUsersByUserId(string userId)
        {
            var bookmarkedUsers = Database.BookmarkedUsers.GetAll().Where(bookmarkedUser => bookmarkedUser.AppUserId == userId).Include(bookmarkedUser => bookmarkedUser.AppUser).Select(tag => new TagViewModel { Name = tag.BookmarkedAppUser.UserName, Type = "User", Area = tag.BookmarkedAppUser.CompanyPosition, LogoSource = tag.BookmarkedAppUser.Photo, BookmarkedData = tag.BookmarkedData }).ToList();
            return bookmarkedUsers;
        }

        public void ConnectUser(ConnectedUserViewModel model)
        {
            Database.ConnectedUsers.Add(Mapping.Map<ConnectedUserViewModel, ConnectedUser>(model));
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public bool CheckIfIsBookmarkedJob(string userId, int jobId)
        {
            return Database.BookmarkedJobs.GetAll().Any(bookmarkedModel => bookmarkedModel.AppUserId == userId && bookmarkedModel.JobId == jobId);
        }

        public bool CheckIfIsConnectedPerson(string userId, string connectedAppUserId)
        {
            return Database.ConnectedUsers.GetAll().Any(bookmarkedModel => bookmarkedModel.AppUserId == userId && bookmarkedModel.ConnectedAppUserId == connectedAppUserId);
        }

        public bool CheckIfIsBookmarkedUser(string currentUserId, string userId)
        {
            return Database.BookmarkedUsers.GetAll().Any(bookmarkedModel => bookmarkedModel.AppUserId == currentUserId && bookmarkedModel.BookmarkedAppUserId == userId);
        }

        public void UpdateUserProfile(string userId, EditUserProfileViewModel model, string oldUserPhoto)
        {
            var user = _userManager.Users.Include(u => u.AppUserSkills).FirstOrDefault(u => u.Id == userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.CompanyPosition = model.CompanyPosition;
            user.Status = model.Status;
            user.Location = model.Location;
            user.Intro = model.Intro;
            user.Passion = model.Passion;
            user.Salary = model.Salary;
            user.PersonalityMatch = model.PersonalityMatch;
            user.JobType = model.JobType;
            user.CV = model.CV;
            user.Experience = model.Experience;
            user.Industry = model.Industry;

            // save photo
            if (String.IsNullOrEmpty(model.Photo))
            {
                // delete if existed
                if (File.Exists(_hostingEnvironment.WebRootPath + user.Photo))
                    File.Delete(_hostingEnvironment.WebRootPath + user.Photo);
                user.Photo = "";
            }
            // save image to folder
            else if (model.Photo != oldUserPhoto)
            {
                string oldPath = user.Photo;

                int numberOfPhoto = 0;
                if (!String.IsNullOrEmpty(oldPath) && Int32.TryParse(oldPath.Split('_').LastOrDefault()?.Split('.').FirstOrDefault(), out numberOfPhoto))
                    numberOfPhoto++;

                string filePath = "/images/Users/" + user.Id + "_" + numberOfPhoto + ".png";

                var bytes = Convert.FromBase64String(model.Photo);
                using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                user.Photo = filePath;
            }
            
            //Accomplishments
            foreach (var item in Database.Accomplishments.GetAll())
            {
                if (item.AppUserId == user.Id)
                    Database.Accomplishments.Delete(item);
            }
            Database.Save();
            foreach (var item in model.Accomplishments)
            {
                Database.Accomplishments.Add(new Accomplishment { Content = item, AppUserId = user.Id });
            }
            

            //SKILLS
            //delete all user's skils
            foreach (var item in Database.AppUserSkills.GetAll())
            {
                if (item.AppUserId == user.Id)
                    Database.AppUserSkills.Delete(item);
            }
            Database.Save();
            //update user's skills
            foreach (var item in model.Skills)
            {
                var skillDb = Database.Skills.GetAll().FirstOrDefault(s => s.Name == item.Name);
                if (skillDb == null)
                {
                    var createdSkill = Database.Skills.Add(new Skill { Name = item.Name });
                    Database.AppUserSkills.Add(new AppUserSkill { AppUser = user, Skill = createdSkill, IsTop = item.IsTop });
                }
                else
                {
                    var userSkillDb = Database.AppUserSkills.GetAll().FirstOrDefault(userSkill => userSkill.SkillId == skillDb.Id && userSkill.AppUserId == user.Id);
                    if (userSkillDb == null)
                    {
                        Database.AppUserSkills.Add(new AppUserSkill { AppUser = user, Skill = skillDb, IsTop = item.IsTop });
                    }
                }
            }
            Database.Save();
            //CERTIFICATES
            //delete 
            foreach (var item in Database.AppUserCertificates.GetAll())
            {
                if (item.AppUserId == user.Id)
                    Database.AppUserCertificates.Delete(item);
            }
            Database.Save();
            //update user's certificates
            foreach (var item in model.Certificates)
            {
                var certificateDb = Database.Certificates.GetAll().FirstOrDefault(s => s.Name == item);
                if (certificateDb == null)
                {
                    var createdCertificate = Database.Certificates.Add(new Certificate { Name = item });
                    Database.AppUserCertificates.Add(new AppUserCertificate { AppUser = user, Certificate = createdCertificate });
                }
                else
                {
                    var userCertificateDb = Database.AppUserCertificates.GetAll().FirstOrDefault(userCertificate => userCertificate.CertificateId == certificateDb.Id && userCertificate.AppUserId == user.Id);
                    if (userCertificateDb == null)
                    {
                        //var createdCertificate = Database.Certificates.Add(new Certificate { Name = item });
                        Database.AppUserCertificates.Add(new AppUserCertificate { AppUser = user, Certificate = certificateDb });
                    }
                }
            }
            Database.Save();
            //Educations
            //delete 
            foreach (var item in Database.AppUserEducations.GetAll())
            {
                if (item.AppUserId == user.Id)
                    Database.AppUserEducations.Delete(item);
            }
            Database.Save();
            //update user's certificates
            foreach (var item in model.Educations)
            {
                var educationDb = Database.Educations.GetAll().FirstOrDefault(s => s.Name == item);
                if (educationDb == null)
                {
                    var createdEducation = Database.Educations.Add(new Education { Name = item });
                    Database.AppUserEducations.Add(new AppUserEducation { AppUser = user, Education = createdEducation });
                }
                else
                {
                    var userEducationDb = Database.AppUserEducations.GetAll().FirstOrDefault(userEducation => userEducation.EducationId == educationDb.Id && userEducation.AppUserId == user.Id);
                    if (userEducationDb == null)
                    {
                        //var createdCertificate = Database.Certificates.Add(new Certificate { Name = item });
                        Database.AppUserEducations.Add(new AppUserEducation { AppUser = user, Education = educationDb });
                    }
                }
            }
            Database.Save();
            //LastCompanies
            //delete 
            foreach (var item in Database.CompanyAppUsers.GetAll())
            {
                if (item.AppUserId == user.Id && item.IsWorkingNow == false)
                    Database.CompanyAppUsers.Delete(item);
            }
            Database.Save();
            //update user's companies
            foreach (var item in model.PastCompanies)
            {
                var companyDb = Database.Companies.GetAll().FirstOrDefault(s => s.Name == item);
                if (companyDb == null)
                {
                    var createdCompany = Database.Companies.Add(new Company { Name = item });
                    Database.CompanyAppUsers.Add(new CompanyAppUser { AppUser = user, Company = createdCompany, IsWorkingNow = false });
                }
                else
                {
                    var userCompanyDb = Database.CompanyAppUsers.GetAll().FirstOrDefault(userCompany => userCompany.CompanyId == companyDb.Id && userCompany.AppUserId == user.Id);
                    if (userCompanyDb == null)
                    {
                        Database.CompanyAppUsers.Add(new CompanyAppUser { AppUser = user, Company = companyDb, IsWorkingNow = false });
                    }
                }
            }
            Database.Save();
            //PresentCompanies
            //delete 
            foreach (var item in Database.CompanyAppUsers.GetAll())
            {
                if (item.AppUserId == user.Id && item.IsWorkingNow == true)
                    Database.CompanyAppUsers.Delete(item);
            }
            //update user's companies
            Database.Save();
            foreach (var item in model.PresentCompanies)
            {
                var companyDb = Database.Companies.GetAll().FirstOrDefault(s => s.Name == item);
                if (companyDb == null)
                {
                    var createdCompany = Database.Companies.Add(new Company { Name = item });
                    Database.CompanyAppUsers.Add(new CompanyAppUser { AppUser = user, Company = createdCompany, IsWorkingNow = true });
                }
                else
                {
                    var userCompanyDb = Database.CompanyAppUsers.GetAll().FirstOrDefault(userCompany => userCompany.CompanyId == companyDb.Id && userCompany.AppUserId == user.Id);
                    if (userCompanyDb == null)
                    {
                        Database.CompanyAppUsers.Add(new CompanyAppUser { AppUser = user, Company = companyDb, IsWorkingNow = true });
                    }
                }
            }

            Database.Save();
        }

        public object GetAPIFilters()
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

        public IEnumerable<AppUser> APIFilter(PeopleFilterViewModel filter)
        {
            IEnumerable<AppUser> result = _userManager.Users.Include(user => user.CompanyAppUsers).ThenInclude(companyAppUser => companyAppUser.Company).Include(user => user.AppUserSkills).ThenInclude(appUserSkill => appUserSkill.Skill).Include(user => user.AppUserCertificates).ThenInclude(appUserCertificate => appUserCertificate.Certificate);

            // sort by location
            if (!string.IsNullOrEmpty(filter.Location))
            {
                result = result.Where(user => user.Location != null && user.Location.Trim().Contains(filter.Location.Trim(), StringComparison.InvariantCultureIgnoreCase));
            }

            // sort by industry
            if (!string.IsNullOrEmpty(filter.Industry))
            {
                result = result.Where(user => user.Industry.GetDescription() == filter.Industry);
            }

            // sort by jobTypes
            if (filter.JobTypes != null && filter.JobTypes.Count() != 0)
            {
                result = result.Where(user => filter.JobTypes.Contains(user.JobType.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by skills
            if (filter.Skills != null && filter.Skills.Count() != 0)
            {
                result = result.Where(user => user.AppUserSkills.Any(appUserSkill => filter.Skills.Any(skill => skill.Equals(appUserSkill.Skill.Name, StringComparison.InvariantCultureIgnoreCase))));
            }

            // sort by experience
            if (filter.Experience != null && filter.Experience.Count() != 0)
            {
                result = result.Where(user => filter.Experience.Contains(user.Experience.GetDescription(), StringComparer.InvariantCultureIgnoreCase));
            }

            // sort by certificates
            if (filter.Certificates != null && filter.Certificates.Count() != 0)
            {
                result = result.Where(user => user.AppUserCertificates.Any(appUserCertificates => filter.Certificates.Any(certificate => certificate.Equals(appUserCertificates.Certificate.Name, StringComparison.InvariantCultureIgnoreCase))));
            }

            // sort by company name
            if (!string.IsNullOrEmpty(filter.CompanyName))
            {
                result = result.Where(user => user.CompanyAppUsers.Any(companyAppUser => companyAppUser.Company != null && companyAppUser.Company.Name.Contains(filter.CompanyName.Trim(), StringComparison.InvariantCultureIgnoreCase)));
            }

            return result;
        }

        public List<string> GetLocations()
        {
            return _userManager.Users.Where(user => !String.IsNullOrEmpty(user.Location)).Select(user => user.Location).Distinct().ToList();
        }

        public void SetEthereumAddress(string userId, string model)
        {
            var user = _userManager.Users.Include(u => u.AppUserSkills).FirstOrDefault(u => u.Id == userId);

            user.EthereumAddress = model;

            Database.Save();
        }
    }
}