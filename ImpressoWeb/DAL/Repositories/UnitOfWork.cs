using System;
using System.Linq;
using DAL.Interfaces;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ImpressoDbContext _context;

        private IRepository<Accomplishment> _accomplishmentRepository;
        private IRepository<AppUserCertificate> _appUserCertificateRepository;
        private IRepository<AppUserEducation> _appUserEducationRepository;
        private IRepository<AppUserJob> _appUserJobRepository;
        private IRepository<AppUserSkill> _appUserSkillsRepository;
        private IRepository<BlockedUser> _blockedUserRepository;
        private IRepository<BookmarkedUser> _bookmarkedUserRepository;
        private IRepository<BookmarkedJob> _bookmarkedJobRepository;
        private IRepository<Certificate> _certificateRepository;
        private IRepository<Education> _educationRepository;
        private IRepository<Company> _companiesRepository;
        private IRepository<CompanyAppUser> _companyAppUsersRepository;
        private IRepository<CompanyCertificate> _companyCertificateRepository;
        private IRepository<ConnectedUser> _connectedUserRepository;
        private IRepository<Event> _eventRepository;
        private IRepository<InterestedEvent> _interestedEventRepository;
        private IRepository<Job> _jobsRepository;
        private IRepository<JobCertificate> _jobCertificateRepository;
        private IRepository<JobSkill> _jobSkillsRepository;
        private IRepository<Log> _loggsRepository;
        private IRepository<Notification> _notificationsRepository;
        private IRepository<Project> _projectsRepository;
        private IRepository<ProjectAppUser> _projectAppUsersRepository;
        private IRepository<Skill> _skillsRepository;
        private IRepository<Testimonial> _testimonialRepository;
        private IRepository<TokenLog> _tokenLogRepository;
        private IRepository<Verificator> _verificatorRepository;


        public UnitOfWork(ImpressoDbContext context)
        {
            _context = context;
        }

        public IRepository<Accomplishment> Accomplishments => _accomplishmentRepository ?? (_accomplishmentRepository = new Repository<Accomplishment>(_context.Accomplishments));
        public IRepository<AppUserCertificate> AppUserCertificates => _appUserCertificateRepository ?? (_appUserCertificateRepository = new Repository<AppUserCertificate>(_context.AppUserCertificates));
        public IRepository<AppUserEducation> AppUserEducations => _appUserEducationRepository ?? (_appUserEducationRepository = new Repository<AppUserEducation>(_context.AppUserEducations));
        public IRepository<AppUserJob> AppUserJobs => _appUserJobRepository ?? (_appUserJobRepository = new Repository<AppUserJob>(_context.AppUserJobs));
        public IRepository<AppUserSkill> AppUserSkills => _appUserSkillsRepository ?? (_appUserSkillsRepository = new Repository<AppUserSkill>(_context.AppUserSkills));
        public IRepository<BlockedUser> BlockedUsers => _blockedUserRepository ?? (_blockedUserRepository = new Repository<BlockedUser>(_context.BlockedUsers));
        public IRepository<BookmarkedUser> BookmarkedUsers => _bookmarkedUserRepository ?? (_bookmarkedUserRepository = new Repository<BookmarkedUser>(_context.BookmarkedUsers));
        public IRepository<BookmarkedJob> BookmarkedJobs => _bookmarkedJobRepository ?? (_bookmarkedJobRepository = new Repository<BookmarkedJob>(_context.BookmarkedJobs));
        public IRepository<Certificate> Certificates => _certificateRepository ?? (_certificateRepository = new Repository<Certificate>(_context.Certificates));
        public IRepository<Education> Educations => _educationRepository ?? (_educationRepository = new Repository<Education>(_context.Educations));

        public IRepository<Company> Companies => _companiesRepository ?? (_companiesRepository = new Repository<Company>(_context.Companies));
        public IRepository<CompanyAppUser> CompanyAppUsers => _companyAppUsersRepository ?? (_companyAppUsersRepository = new Repository<CompanyAppUser>(_context.CompanyAppUsers));
        public IRepository<CompanyCertificate> CompanyCertificates => _companyCertificateRepository ?? (_companyCertificateRepository = new Repository<CompanyCertificate>(_context.CompanyCertificates));
        public IRepository<ConnectedUser> ConnectedUsers => _connectedUserRepository ?? (_connectedUserRepository = new Repository<ConnectedUser>(_context.ConnectedUsers));
        public IRepository<Event> Events => _eventRepository ?? (_eventRepository = new Repository<Event>(_context.Events));
        public IRepository<InterestedEvent> InterestedEvents => _interestedEventRepository ?? (_interestedEventRepository = new Repository<InterestedEvent>(_context.InterestedEvents));
        public IRepository<Job> Jobs => _jobsRepository ?? (_jobsRepository = new Repository<Job>(_context.Jobs));
        public IRepository<JobCertificate> JobCertificates => _jobCertificateRepository ?? (_jobCertificateRepository = new Repository<JobCertificate>(_context.JobCertificates));
        public IRepository<JobSkill> JobSkills => _jobSkillsRepository ?? (_jobSkillsRepository = new Repository<JobSkill>(_context.JobSkills));
        public IRepository<Log> Loggs => _loggsRepository ?? (_loggsRepository = new Repository<Log>(_context.Loggs));
        public IRepository<Notification> Notifications => _notificationsRepository ?? (_notificationsRepository = new Repository<Notification>(_context.Notifications));
        public IRepository<Project> Projects => _projectsRepository ?? (_projectsRepository = new Repository<Project>(_context.Projects));
        public IRepository<ProjectAppUser> ProjectAppUsers => _projectAppUsersRepository ?? (_projectAppUsersRepository = new Repository<ProjectAppUser>(_context.ProjectAppUsers));
        public IRepository<Skill> Skills => _skillsRepository ?? (_skillsRepository = new Repository<Skill>(_context.Skills));
        public IRepository<Testimonial> Testimonials => _testimonialRepository ?? (_testimonialRepository = new Repository<Testimonial>(_context.Testimonials));
        public IRepository<TokenLog> TokenLogs => _tokenLogRepository ?? (_tokenLogRepository = new Repository<TokenLog>(_context.TokenLogs));
        public IRepository<Verificator> Verificators => _verificatorRepository ?? (_verificatorRepository = new Repository<Verificator>(_context.Verificators));


        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}