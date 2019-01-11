using System.Linq;
using AutoMapper;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BLL.AutoMapper
{
    public static class Mapping
    {
        public static TResult Map<TSource, TResult>(TSource source, Action<IMappingOperationOptions<TSource, TResult>> opts = null)
        {
            if (opts != null)
                return Mapper.Map<TSource, TResult>(source, opts);
            return Mapper.Map<TSource, TResult>(source);
        }
        static Mapping()
        {
            Mapper.Initialize(config => {

                config.CreateMap<AppUserJob, AppUserJobViewModel>();
                config.CreateMap<AppUserJobViewModel, AppUserJob>();

                config.CreateMap<AppUser, AppUserViewModel>();
                config.CreateMap<AppUserViewModel, AppUser>();

                config.CreateMap<Certificate, CertificateViewModel>();
                config.CreateMap<CertificateViewModel, Certificate>();

                config.CreateMap<JobCertificate, JobCertificateViewModel>();
                config.CreateMap<JobCertificateViewModel, JobCertificate>();

                config.CreateMap<AppUserJob, ApplicantViewModel>()
                .ForMember(applicant => applicant.Id, opts => opts.MapFrom(appUserJob => appUserJob.AppUser.Id))
                .ForMember(applicant => applicant.Photo, opts => opts.MapFrom(appUserJob => appUserJob.AppUser.Photo))
                .ForMember(applicant => applicant.UserName, opts => opts.MapFrom(appUserJob => appUserJob.AppUser.UserName))
                .ForMember(applicant => applicant.Profession, opts => opts.MapFrom(appUserJob => appUserJob.AppUser.Profession))
                .ForMember(applicant => applicant.JobName, opts => opts.MapFrom(appUserJob => appUserJob.Job.Name))
                .ForMember(applicant => applicant.ProjectName, opts => opts.MapFrom(appUserJob => appUserJob.Job.Project.Name));
                config.CreateMap<ApplicantViewModel, AppUserJob>();

                config.CreateMap<ProjectAppUser, ProjectAppUserViewModel>();
                config.CreateMap<ProjectAppUserViewModel, ProjectAppUser>();

                config.CreateMap<TokenLog, TokenLogViewModel>()
                .ForMember(tokenLogViewModel => tokenLogViewModel.Sender, opts => opts.MapFrom(tokenLog => tokenLog.Sender.UserName))
                .ForMember(tokenLogViewModel => tokenLogViewModel.Receiver, opts => opts.MapFrom(tokenLog => tokenLog.Receiver.UserName));
                config.CreateMap<TokenLogViewModel, TokenLog>();

                config.CreateMap<InterestedEvent, InterestedEventViewModel>();
                config.CreateMap<InterestedEventViewModel, InterestedEvent>();

                config.CreateMap<AppUser, PeopleConnectViewModel>()
                .ForMember(people => people.ID, conf => conf.MapFrom(appUser => appUser.Id))
                .ForMember(people => people.PictureSource, conf => conf.MapFrom(appUser => appUser.Photo))
                .ForMember(people => people.CityAddress, conf => conf.MapFrom(appUser => appUser.Location))
                .ForMember(people => people.YearsOfExperiense, conf => conf.MapFrom(appUser => appUser.Experience))
                .ForMember(people => people.Name, conf  => conf.MapFrom(appUser => appUser.UserName))
                .ForMember(people => people.Description, conf => conf.MapFrom(appUser => appUser.AdditionalInformation))
                .ForMember(people => people.Position, conf => conf.MapFrom(appUser => appUser.Profession))
                .ForMember(people => people.FullName, conf => conf.MapFrom(appUser => appUser.FirstName + " " + appUser.LastName))
                .ForMember(people => people.IsBookmarked,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.IsBookmarked = src.BookmarkedByUsers.Any(bookmarkedUser => bookmarkedUser.AppUserId == resContext.Items["CurrentUserId"].ToString())))
                .ForMember(people => people.IsConnected,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.IsConnected = src.ConnectedByUsers.Any(connectedUser => connectedUser.AppUserId == resContext.Items["CurrentUserId"].ToString())));
                config.CreateMap<PeopleConnectViewModel, AppUser>();


                config.CreateMap<Event, EventViewModel>()
                .ForMember(eventViewModel => eventViewModel.HostedById, conf => conf.MapFrom(ev => ev.Company.Id))
                .ForMember(eventViewModel => eventViewModel.HostedByName, conf => conf.MapFrom(ev => ev.Company.Name));
                config.CreateMap<EventViewModel, Event>()
                .ForMember(ev => ev.CompanyId, conf => conf.MapFrom(ev => ev.HostedById));

                config.CreateMap<Skill, SkillViewModel>();
                config.CreateMap<SkillViewModel, Skill>();

                config.CreateMap<AppUserSkillViewModel, AppUserSkill>();
                config.CreateMap<AppUserSkill, AppUserSkillViewModel>();

                config.CreateMap<Job, JobViewModel>()
                .ForMember(jobViewModel => jobViewModel.Skills, conf => conf.MapFrom(job => job.JobSkills.Where(jobSkill => jobSkill.JobId == job.Id).Select(jobSKill => jobSKill.Skill.Name)))
                .ForMember(jobViewModel => jobViewModel.Title, conf => conf.MapFrom(job => job.Name))
                .ForMember(jobViewModel => jobViewModel.Level, conf => conf.MapFrom(job => job.Experience))
                .ForMember(jobViewModel => jobViewModel.PostedTime, conf => conf.MapFrom(job => job.DateOfPost))
                .ForMember(jobViewModel => jobViewModel.ProjectName, conf => conf.MapFrom(job => job.Project.Name))
                .ForMember(jobViewModel => jobViewModel.ApplicantsCount, conf => conf.MapFrom(job => job.AppUserJobs.Count))
                .ForMember(jobViewModel => jobViewModel.CompanyId, conf => conf.MapFrom(job => job.Project.CompanyId));
                config.CreateMap<JobViewModel, Job>()
                    .ForMember(job => job.Name, conf => conf.MapFrom(jobViewModel => jobViewModel.Title));

                config.CreateMap<Job, JobDetailsViewModel>()
                .ForMember(jobViewModel => jobViewModel.Skills, conf => conf.MapFrom(job => job.JobSkills.Select(jobSKill => jobSKill.Skill.Name)))
                .ForMember(jobViewModel => jobViewModel.JobCertificates, conf => conf.MapFrom(job => job.JobCertificates.Select(jobCertificate => jobCertificate.Certificate.Name)))
                .ForMember(jobViewModel => jobViewModel.AppUsers, conf => conf.MapFrom(job => job.AppUserJobs.Select(appUserJob => appUserJob.AppUser)));
                config.CreateMap<JobDetailsViewModel, Job>()
                .ForMember(job => job.JobSkills, conf => conf.MapFrom(jobViewModel => jobViewModel.Skills.Select(skill => new JobSkill { SkillId = Convert.ToInt32(skill), JobId = jobViewModel.Id })))
                .ForMember(job => job.JobCertificates, conf => conf.MapFrom(jobViewModel => jobViewModel.JobCertificates.Select(certificate => new JobCertificateViewModel() { CertificateId = Convert.ToInt32(certificate), JobId = jobViewModel.Id })));

                config.CreateMap<CompanyViewModel, Company>()
                .ForMember(company => company.Name, conf => conf.MapFrom(companyViewModel => companyViewModel.CompanyName))
                .ForMember(company => company.Photo, conf => conf.MapFrom(companyViewModel => companyViewModel.CompanyLogoSource))
                .ForMember(company => company.Intro, conf => conf.MapFrom(companyViewModel => companyViewModel.WhoWeAreText))
                .ForMember(company => company.Industry, conf => conf.MapFrom(companyViewModel => companyViewModel.CompanyArea));
                config.CreateMap<Company, CompanyViewModel>()
                .ForMember(companyViewModel => companyViewModel.CompanyName, conf => conf.MapFrom(company => company.Name))
                .ForMember(companyViewModel => companyViewModel.CompanyArea, conf => conf.MapFrom(company => company.Industry))
                .ForMember(companyViewModel => companyViewModel.CompanyLogoSource, conf => conf.MapFrom(company => company.Photo))
                .ForMember(companyViewModel => companyViewModel.WhoWeAreText, conf => conf.MapFrom(company => company.Intro))
                .ForMember(companyViewModel => companyViewModel.EmployeesCount, conf => conf.MapFrom(company => company.CompanyAppUsers.Count()))
                .ForMember(companyViewModel => companyViewModel.Vacancies,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.Vacancies = src.Projects != null ? Mapping.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(src.Projects.SelectMany(project => project.Jobs)) : null));

                config.CreateMap<NotificationViewModel, Notification>();
                config.CreateMap<Notification, NotificationViewModel>();

                config.CreateMap<CompanyAppUserViewModel, CompanyAppUser>();
                config.CreateMap<CompanyAppUser, CompanyAppUserViewModel>();

                config.CreateMap<ProjectViewModel, Project>();
                config.CreateMap<Project, ProjectViewModel>()
                .ForMember(projectViewModel => projectViewModel.AmountOfCandidates, opts => opts.MapFrom(project => project.ProjectAppUsers.Count));

                config.CreateMap<CreateJobViewModel, Job>()
                .ForMember(job => job.Name, conf => conf.MapFrom(jobViewModel => jobViewModel.Title))
                .ForMember(job => job.Experience, conf => conf.MapFrom(jobViewModel => jobViewModel.Level))
                .ForMember(job => job.DateOfPost, conf => conf.MapFrom(jobViewModel => jobViewModel.PostedTime));
                config.CreateMap<Job, CreateJobViewModel>();

                config.CreateMap<BlockedUserViewModel, BlockedUser>();
                config.CreateMap<BlockedUser, BlockedUserViewModel>();

                config.CreateMap<BookmarkedJobViewModel, BookmarkedJob>();
                config.CreateMap<BookmarkedJob, BookmarkedJobViewModel>();

                config.CreateMap<ConnectedUserViewModel, ConnectedUser>();
                config.CreateMap<ConnectedUser, ConnectedUserViewModel>();

                config.CreateMap<BookmarkedUserViewModel, BookmarkedUser>();
                config.CreateMap<BookmarkedUser, BookmarkedUserViewModel>();


                config.CreateMap<AppUserCertificateViewModel, AppUserCertificate>();
                config.CreateMap<AppUserCertificate, AppUserCertificateViewModel>();

                config.CreateMap<TestimonialViewModel, Testimonial>();
                config.CreateMap<Testimonial, TestimonialViewModel>()
                .ForMember(testimonialViewModel => testimonialViewModel.RecommenderName, conf => conf.MapFrom(testimonial => testimonial.Recommender.UserName))
                .ForMember(testimonialViewModel => testimonialViewModel.RecommenderCompanyPosition, conf => conf.MapFrom(testimonial => testimonial.Recommender.CompanyPosition))
                .ForMember(testimonialViewModel => testimonialViewModel.RecommenderCompanyName, conf => conf.MapFrom(testimonial => testimonial.Recommender.CompanyAppUsers.FirstOrDefault().Company.Name))
                .ForMember(testimonialViewModel => testimonialViewModel.RecommendedUserName, conf => conf.MapFrom(testimonial => testimonial.RecommendedUser.UserName))
                .ForMember(testimonialViewModel => testimonialViewModel.RecommendedUserPhoto, conf => conf.MapFrom(testimonial => testimonial.RecommendedUser.Photo))
                .ForMember(testimonialViewModel => testimonialViewModel.RecommendedUserCompanyPosition, conf => conf.MapFrom(testimonial => testimonial.RecommendedUser.CompanyPosition))
                .ForMember(testimonialViewModel => testimonialViewModel.IsVerified, 
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.IsVerified = src.Verificators.Any(verificator => verificator.AppUserId == resContext.Items["CurrentUserId"].ToString())));

                config.CreateMap<VerificatorViewModel, Verificator>();
                config.CreateMap<Verificator, VerificatorViewModel>()
                    .ForMember(verificatorViewModel => verificatorViewModel.AppUserPhoto, opts => opts.MapFrom(verificator => verificator.AppUser.Photo));

                config.CreateMap<CompanyProfileViewModel, Company>();
                config.CreateMap<Company, CompanyProfileViewModel>()
                .ForMember(companyProfile => companyProfile.Jobs,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.Jobs = Mapping.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(src.Projects.SelectMany(project => project.Jobs))))
                .ForMember(companyProfile => companyProfile.Tokens,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.Tokens = Mapping.Map<IEnumerable<TokenLog>, IEnumerable<TokenLogViewModel>>(src.CompanyAppUsers.First(companyAppUser => companyAppUser.AppUserId == resContext.Items["CurrentUserId"].ToString()).AppUser.ReceivedTokens)));

                config.CreateMap<ConnectPersonCompanyViewModel, Company>();
                config.CreateMap<Company, ConnectPersonCompanyViewModel>()
                    .ForMember(connectPersonCompanyViewModel => connectPersonCompanyViewModel.Jobs,
                        conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                            dest.Jobs = Mapping.Map<IEnumerable<Job>, IEnumerable<JobViewModel>>(src.Projects.SelectMany(project => project.Jobs))));

                config.CreateMap<SavePersonForProjectViewModel, Company>();
                config.CreateMap<Company, SavePersonForProjectViewModel>()
                    .ForMember(savePersonForProjectViewModel => savePersonForProjectViewModel.Projects,
                        conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                            dest.Projects = Mapping.Map<IEnumerable<Project>, IEnumerable<ProjectViewModel>>(src.Projects)));


                config.CreateMap<PersonProfileViewModel, AppUser>();
                config.CreateMap<AppUser, PersonProfileViewModel>()
                .ForMember(personProfile => personProfile.fullName, conf => conf.MapFrom(user => user.FirstName + " " + user.LastName))
                .ForMember(personProfile => personProfile.Skills, conf => conf.MapFrom(user => user.AppUserSkills.Select(appUserSkill => new SkillViewModel() { Name = appUserSkill.Skill.Name, IsTop = appUserSkill.IsTop})))
                .ForMember(personProfile => personProfile.Certificates, conf => conf.MapFrom(user => user.AppUserCertificates.Select(appUserCertificate => appUserCertificate.Certificate.Name)))
                .ForMember(personProfile => personProfile.Educations, conf => conf.MapFrom(user => user.AppUserEducations.Select(appUserEducation => appUserEducation.Education.Name)))
                .ForMember(personProfile => personProfile.PresentCompanies, conf => conf.MapFrom(user => user.CompanyAppUsers.Where(companyAppUser => companyAppUser.IsWorkingNow == true).Select(companyAppUser => companyAppUser.Company.Name)))
                .ForMember(personProfile => personProfile.PastCompanies, conf => conf.MapFrom(user => user.CompanyAppUsers.Where(companyAppUser => companyAppUser.IsWorkingNow == false).Select(companyAppUser => companyAppUser.Company.Name)))
                .ForMember(personProflie => personProflie.Testimonials, conf => conf.MapFrom(user => user.ReceivedTestimonials))
                .ForMember(personProfile => personProfile.Accomplishments, conf => conf.MapFrom(user => user.Accomplishments.Select(accomplishment => accomplishment.Content)))
                .ForMember(personProfile => personProfile.Connections, conf => conf.MapFrom(user => user.ConnectedUsers.Count))
                .ForMember(people => people.IsBookmarked,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.IsBookmarked = src.BookmarkedByUsers.Any(bookmarkedUser => bookmarkedUser.AppUserId == resContext.Items["CurrentUserId"].ToString())))
                .ForMember(people => people.IsConnected,
                    conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    dest.IsConnected = src.ConnectedByUsers.Any(connectedUser => connectedUser.AppUserId == resContext.Items["CurrentUserId"].ToString())));
            });
        }
    }
}