using System;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Extensions;

namespace Core
{
    public class ImpressoDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<Accomplishment> Accomplishments { get; set; }
        public virtual DbSet<AppUserCertificate> AppUserCertificates { get; set; }
        public virtual DbSet<AppUserEducation> AppUserEducations { get; set; }
        public virtual DbSet<AppUserJob> AppUserJobs { get; set; }
        public virtual DbSet<AppUserSkill> AppUserSkills { get; set; }
        public virtual DbSet<BlockedUser> BlockedUsers { get; set; }
        public virtual DbSet<BookmarkedUser> BookmarkedUsers { get; set; }
        public virtual DbSet<BookmarkedJob> BookmarkedJobs { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyAppUser> CompanyAppUsers { get; set; }
        public virtual DbSet<CompanyCertificate> CompanyCertificates { get; set; }
        public virtual DbSet<ConnectedUser> ConnectedUsers { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<InterestedEvent> InterestedEvents { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobCertificate> JobCertificates { get; set; }
        public virtual DbSet<JobSkill> JobSkills { get; set; }
        public virtual DbSet<Log> Loggs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectAppUser> ProjectAppUsers { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<TokenLog> TokenLogs { get; set; }        
        public virtual DbSet<Verificator> Verificators { get; set; }


        public ImpressoDbContext(DbContextOptions<ImpressoDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // many to many
            // AppUserCertificates
            modelBuilder.Entity<AppUserCertificate>()
                .HasKey(appUserCertificates => new { appUserCertificates.AppUserId, appUserCertificates.CertificateId });

            modelBuilder.Entity<AppUserCertificate>()
                .HasOne(appUserCertificate => appUserCertificate.AppUser)
                .WithMany(appUser => appUser.AppUserCertificates)
                .HasForeignKey(appUserCertificate => appUserCertificate.AppUserId);

            modelBuilder.Entity<AppUserCertificate>()
                .HasOne(appUserCertificate => appUserCertificate.Certificate)
                .WithMany(certificate => certificate.AppUserCertificates)
                .HasForeignKey(appUserCertificate => appUserCertificate.CertificateId);

            // AppUserEdications
            modelBuilder.Entity<AppUserEducation>()
                .HasKey(appUserEducations => new { appUserEducations.AppUserId, appUserEducations.EducationId });

            modelBuilder.Entity<AppUserEducation>()
                .HasOne(appUserEducation => appUserEducation.AppUser)
                .WithMany(appUser => appUser.AppUserEducations)
                .HasForeignKey(appUserEducation => appUserEducation.AppUserId);

            modelBuilder.Entity<AppUserEducation>()
                .HasOne(appUserEducation => appUserEducation.Education)
                .WithMany(education => education.AppUserEducations)
                .HasForeignKey(appUserEducation => appUserEducation.EducationId);

            // AppUserJobs
            modelBuilder.Entity<AppUserJob>()
                .HasKey(appUserCertificates => new { appUserCertificates.AppUserId, appUserCertificates.JobId });

            modelBuilder.Entity<AppUserJob>()
                .HasOne(appUserJob => appUserJob.AppUser)
                .WithMany(appUser => appUser.AppUserJobs)
                .HasForeignKey(appUserJob => appUserJob.AppUserId);

            modelBuilder.Entity<AppUserJob>()
                .HasOne(appUserJob => appUserJob.Job)
                .WithMany(job => job.AppUserJobs)
                .HasForeignKey(appUserJob => appUserJob.JobId);

            // AppUserSkills
            modelBuilder.Entity<AppUserSkill>()
                .HasKey(appUserSkill => new { appUserSkill.AppUserId, appUserSkill.SkillId });

            modelBuilder.Entity<AppUserSkill>()
                .HasOne(appUserSkill => appUserSkill.AppUser)
                .WithMany(appUser => appUser.AppUserSkills)
                .HasForeignKey(appUserSkill => appUserSkill.AppUserId);

            modelBuilder.Entity<AppUserSkill>()
                .HasOne(appUserSkill => appUserSkill.Skill)
                .WithMany(skill => skill.AppUserSkills)
                .HasForeignKey(appUserSkill => appUserSkill.SkillId);

            // BookmarkedJob
            modelBuilder.Entity<BookmarkedJob>()
                .HasKey(bookmarkedJob => new { bookmarkedJob.AppUserId, bookmarkedJob.JobId });

            modelBuilder.Entity<BookmarkedJob>()
                .HasOne(bookmarkedJob => bookmarkedJob.AppUser)
                .WithMany(appUser => appUser.BookmarkedJobs)
                .HasForeignKey(bookmarkedJob => bookmarkedJob.AppUserId);

            modelBuilder.Entity<BookmarkedJob>()
                .HasOne(bookmarkedJob => bookmarkedJob.Job)
                .WithMany(job => job.BookmarkedJobs)
                .HasForeignKey(bookmarkedJob => bookmarkedJob.JobId);

            // CompanyAppUsers
            modelBuilder.Entity<CompanyAppUser>()
                .HasKey(companyAppUsers => new { companyAppUsers.CompanyId, companyAppUsers.AppUserId });

            modelBuilder.Entity<CompanyAppUser>()
                .HasOne(companyAppUser => companyAppUser.Company)
                .WithMany(company => company.CompanyAppUsers)
                .HasForeignKey(companyAppUser => companyAppUser.CompanyId);

            modelBuilder.Entity<CompanyAppUser>()
                .HasOne(companyAppUser => companyAppUser.AppUser)
                .WithMany(appUser => appUser.CompanyAppUsers)
                .HasForeignKey(companyAppUser => companyAppUser.AppUserId);

            // CompanyCertificates
            modelBuilder.Entity<CompanyCertificate>()
                .HasKey(companyCertificates => new { companyCertificates.CompanyId, companyCertificates.CertificateId });

            modelBuilder.Entity<CompanyCertificate>()
                .HasOne(companyCertificate => companyCertificate.Company)
                .WithMany(company => company.CompanyCertificates)
                .HasForeignKey(companyCertificate => companyCertificate.CompanyId);

            modelBuilder.Entity<CompanyCertificate>()
                .HasOne(companyCertificate => companyCertificate.Certificate)
                .WithMany(certificate => certificate.CompanyCertificates)
                .HasForeignKey(companyCertificate => companyCertificate.CertificateId);

            // InterestedEvent
            modelBuilder.Entity<InterestedEvent>()
                .HasKey(bookmarkedEvent => new { bookmarkedEvent.AppUserId, bookmarkedEvent.EventId });

            modelBuilder.Entity<InterestedEvent>()
                .HasOne(bookmarkedEvent => bookmarkedEvent.AppUser)
                .WithMany(appUser => appUser.InterestedEvents)
                .HasForeignKey(bookmarkedEvent => bookmarkedEvent.AppUserId);

            modelBuilder.Entity<InterestedEvent>()
                .HasOne(bookmarkedEvent => bookmarkedEvent.Event)
                .WithMany(appevent => appevent.InterestedEvents)
                .HasForeignKey(bookmarkedEvent => bookmarkedEvent.EventId);

            // JobCertificates
            modelBuilder.Entity<JobCertificate>()
                .HasKey(jobCertificates => new { jobCertificates.JobId, jobCertificates.CertificateId });

            modelBuilder.Entity<JobCertificate>()
                .HasOne(jobCertificate => jobCertificate.Job)
                .WithMany(job => job.JobCertificates)
                .HasForeignKey(jobCertificate => jobCertificate.JobId);

            modelBuilder.Entity<JobCertificate>()
                .HasOne(jobCertificate => jobCertificate.Certificate)
                .WithMany(certificate => certificate.JobCertificates)
                .HasForeignKey(jobCertificate => jobCertificate.CertificateId);

            // JobSkills
            modelBuilder.Entity<JobSkill>()
                .HasKey(jobSkills => new { jobSkills.JobId, jobSkills.SkillId });

            modelBuilder.Entity<JobSkill>()
                .HasOne(jobSkill => jobSkill.Job)
                .WithMany(job => job.JobSkills)
                .HasForeignKey(jobSkill => jobSkill.JobId);

            modelBuilder.Entity<JobSkill>()
                .HasOne(jobSkill => jobSkill.Skill)
                .WithMany(skill => skill.JobSkills)
                .HasForeignKey(jobSkill => jobSkill.SkillId);

            // ProjectAppUsers
            modelBuilder.Entity<ProjectAppUser>()
                .HasKey(projectAppUsers => new { projectAppUsers.ProjectId, projectAppUsers.AppUserId });

            modelBuilder.Entity<ProjectAppUser>()
                .HasOne(projectAppUser => projectAppUser.Project)
                .WithMany(company => company.ProjectAppUsers)
                .HasForeignKey(projectAppUser => projectAppUser.ProjectId);

            modelBuilder.Entity<ProjectAppUser>()
                .HasOne(projectAppUser => projectAppUser.AppUser)
                .WithMany(appUser => appUser.ProjectAppUsers)
                .HasForeignKey(projectAppUser => projectAppUser.AppUserId);

            
            // many to many on the same table
            // BlockedUsers
            modelBuilder.Entity<BlockedUser>()
                .HasKey(blockedUser => new { blockedUser.AppUserId, blockedUser.BlockedAppUserId });

            modelBuilder.Entity<BlockedUser>()
                .HasOne(blockedUser => blockedUser.AppUser)
                .WithMany(appUser => appUser.BlockedUsers)
                .HasForeignKey(blockedUser => blockedUser.AppUserId);

            modelBuilder.Entity<BlockedUser>()
                .HasOne(blockedUser => blockedUser.BlockedAppUser)
                .WithMany(appUser => appUser.BlockedByUsers)
                .HasForeignKey(blockedUser => blockedUser.BlockedAppUserId);

            // BookmarkedUsers
            modelBuilder.Entity<BookmarkedUser>()
                .HasKey(bookmarkedUser => new { bookmarkedUser.AppUserId, bookmarkedUser.BookmarkedAppUserId });

            modelBuilder.Entity<BookmarkedUser>()
                .HasOne(bookmarkedUser => bookmarkedUser.AppUser)
                .WithMany(appUser => appUser.BookmarkedUsers)
                .HasForeignKey(bookmarkedUser => bookmarkedUser.AppUserId);

            modelBuilder.Entity<BookmarkedUser>()
                .HasOne(bookmarkedUser => bookmarkedUser.BookmarkedAppUser)
                .WithMany(appUser => appUser.BookmarkedByUsers)
                .HasForeignKey(bookmarkedUser => bookmarkedUser.BookmarkedAppUserId);

            // ConnectedUsers
            modelBuilder.Entity<ConnectedUser>()
                .HasKey(connectedUser => new { connectedUser.AppUserId, connectedUser.ConnectedAppUserId });

            modelBuilder.Entity<ConnectedUser>()
                .HasOne(connectedUser => connectedUser.AppUser)
                .WithMany(appUser => appUser.ConnectedUsers)
                .HasForeignKey(connectedUser => connectedUser.AppUserId);

            modelBuilder.Entity<ConnectedUser>()
                .HasOne(connectedUser => connectedUser.ConnectedAppUser)
                .WithMany(appUser => appUser.ConnectedByUsers)
                .HasForeignKey(connectedUser => connectedUser.ConnectedAppUserId);

            // Testimonials
            modelBuilder.Entity<Testimonial>()
                .HasOne(testimonial => testimonial.Recommender)
                .WithMany(appUser => appUser.SentTestimonials)
                .HasForeignKey(testimonial => testimonial.RecommenderId);

            modelBuilder.Entity<Testimonial>()
                .HasOne(testimonial => testimonial.RecommendedUser)
                .WithMany(appUser => appUser.ReceivedTestimonials)
                .HasForeignKey(testimonial => testimonial.RecommendedUserId);

            // TokenLog
            modelBuilder.Entity<TokenLog>()
                .HasOne(tokenLog => tokenLog.Sender)
                .WithMany(appUser => appUser.SendedTokens)
                .HasForeignKey(tokenLog => tokenLog.SenderId);

            modelBuilder.Entity<TokenLog>()
                .HasOne(tokenLog => tokenLog.Receiver)
                .WithMany(appUser => appUser.ReceivedTokens)
                .HasForeignKey(tokenLog => tokenLog.ReceiverId);


            // default values
            modelBuilder.Entity<AppUser>()
                .Property(appUser => appUser.Tokens)
                .HasDefaultValue(0);

            modelBuilder.Entity<CompanyAppUser>()
                .Property(companyAppUser => companyAppUser.IsWorkingNow)
                .HasDefaultValue(true);

            modelBuilder.Entity<Notification>()
                .Property(notification => notification.IsNewest)
                .HasDefaultValue(true);

            modelBuilder.Entity<AppUser>()
                .Property(appUser => appUser.LastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Company>()
                .Property(company => company.LastUpdate)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Job>()
                .Property(job => job.DateOfPost)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Notification>()
                .Property(notification => notification.DateOfPost)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Project>()
                .Property(project => project.StartDate)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Testimonial>()
                .Property(testimonial => testimonial.DateOfPost)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<AppUserJob>()
                .Property(appUserJob => appUserJob.DateOfPost)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<TokenLog>()
                .Property(tokenLog => tokenLog.DepartureDate)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<BookmarkedJob>()
                .Property(bookmarkedJob => bookmarkedJob.BookmarkedData)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<BookmarkedUser>()
                .Property(bookmarkedUser => bookmarkedUser.BookmarkedData)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<InterestedEvent>()
                .Property(bookmarkedEvent => bookmarkedEvent.BookmarkedData)
                .HasDefaultValueSql("NOW()");

            base.OnModelCreating(modelBuilder);
        }

        public static async System.Threading.Tasks.Task CreateAppUserAccount(IServiceProvider serviceProvider,
            IConfiguration configuration, string configPath)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration[configPath + ":Name"];
            string email = configuration[configPath + ":Email"];
            string password = configuration[configPath + ":Password"];
            string role = configuration[configPath + ":Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}