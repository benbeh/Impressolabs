using System.Collections.Generic;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;

namespace BLL.Interfaces
{
    public interface IJobService : IService<Job, JobViewModel>
    {
        IEnumerable<JobViewModel> GetAllByName(string name);

        IEnumerable<JobViewModel> GetListJobs(string userId);

        IList<string> GetTopSkillsMatch(string userId, int jobId);

        JobViewModel GetJobInfo(string userId, int jobId);

        JobDetailsViewModel GetJobDetailsViewModel(int jobId);

        void CreateJob(CreateJobViewModel model);

        void AddUser(string userId, int jobId);

        object GetFilters();

        IEnumerable<TagViewModel> GetBookmakedJobsByUserId(string userId);

        IEnumerable<JobViewModel> Filter(JobFilterViewModel filter);

        bool ApplyForProjectByUserId(AppUserJobViewModel appUserJobViewModel);

        IEnumerable<JobViewModel> ListAppliedJobsOfCurrentUser(string userId);

        void Update(JobDetailsViewModel job);

        List<string> GetLocations();
    }
}