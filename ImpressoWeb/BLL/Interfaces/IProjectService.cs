using BLL.ViewModels.API;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IProjectService : IService<Project, ProjectViewModel>
    {
        void CreateProject(string userId, ProjectViewModel model);

        void AddUser(string userId, int projectId);

        ProjectViewModel GetProjectWithUsersAndJobs(int projectId);

        void ChangeProjectName(int id, string name);
    }
}
