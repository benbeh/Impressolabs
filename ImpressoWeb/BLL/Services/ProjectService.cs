using BLL.Interfaces;
using BLL.ViewModels.API;
using DAL.Interfaces;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProjectService : Service<Project, ProjectViewModel>, IProjectService
    {
        public ProjectService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Projects)
        {

        }

        public void CreateProject(string userId, ProjectViewModel model)
        {
            var companyAppUser = Database.CompanyAppUsers.GetAll().FirstOrDefault(appUser => appUser.AppUserId == userId);

            Project createdProject = new Project()
            {
                Name = model.Name,
                Description = model.Description,
                CompanyId = companyAppUser.CompanyId,
            };

            Database.Projects.Add(createdProject);
            Database.Save();
        }

        public void AddUser(string userId, int projectId)
        {
            if (Database.ProjectAppUsers.GetAll().Any(projectAppUser => projectAppUser.AppUserId == userId && projectAppUser.ProjectId == projectId))
                return;

            Database.ProjectAppUsers.Add(new ProjectAppUser() {AppUserId = userId, ProjectId = projectId});
            Database.Save();
        }

        public ProjectViewModel GetProjectWithUsersAndJobs(int projectId)
        {
            var item = Database.Projects.GetAll().Include(project => project.ProjectAppUsers).ThenInclude(projectAppUser => projectAppUser.AppUser).Include(project => project.Jobs).FirstOrDefault(project => project.Id == projectId);
            if (item == null)
                throw new Exception("Project with id (" + projectId + ") wasn't found");
            return Mapping.Map<Project, ProjectViewModel>(item);
        }

        public void ChangeProjectName(int id, string name)
        {
            var project = Database.Projects.GetAll().AsNoTracking().First(pr => pr.Id == id);
            project.Name = name;
            Database.Projects.Update(project);
            Database.Save();
        }
    }
}
