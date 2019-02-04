using Core.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Accomplishment> Accomplishments { get; }
        IRepository<AppUserCertificate> AppUserCertificates { get; }
        IRepository<AppUserEducation> AppUserEducations { get; }
        IRepository<AppUserJob> AppUserJobs { get; }
        IRepository<AppUserSkill> AppUserSkills { get; }
        IRepository<BlockedUser> BlockedUsers { get; }
        IRepository<BookmarkedUser> BookmarkedUsers { get; }
        IRepository<BookmarkedJob> BookmarkedJobs { get; }
        IRepository<Certificate> Certificates { get; }
        IRepository<Education> Educations { get; }
        IRepository<Company> Companies { get; }
        IRepository<CompanyAppUser> CompanyAppUsers { get; }
        IRepository<CompanyCertificate> CompanyCertificates { get; }
        IRepository<ConnectedUser> ConnectedUsers { get; }
        IRepository<Event> Events { get; }
        IRepository<InterestedEvent> InterestedEvents { get; }
        IRepository<Job> Jobs { get; }
        IRepository<JobCertificate> JobCertificates { get; }
        IRepository<JobSkill> JobSkills { get; }
        IRepository<Log> Loggs { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<Project> Projects { get; }
        IRepository<ProjectAppUser> ProjectAppUsers { get; }
        IRepository<Skill> Skills { get; }
        IRepository<Testimonial> Testimonials { get; }
        IRepository<TokenLog> TokenLogs { get; }
        IRepository<Verificator> Verificators { get; }


        void Save();

        void DetachAllEntities();

        void Dispose(bool disposing);
        void Dispose();
    }
}