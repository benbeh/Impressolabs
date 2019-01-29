using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Models;
using ImpressoApp.Models.Feeds;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.User;

namespace ImpressoApp.Services.Job
{
    public interface IJobService
    {
        Task CreateJob(JobModel jobModel);

        Task<List<JobModel>> GetJobs();

        Task<List<JobModel>> GetFilteredJobs(JobFilterModel filterModel);

        Task<BaseResponseModel> ApplyForJobAsync(int jobId);

        Task<BaseResponseModel> SetAsBookmarkedAsync(SetAsBookmarkedRequestModel model);

        Task<List<JobModel>> GetAppliedJobsAsync();

        Task<JobFiltersServerModel> GetFilters();

        Task<JobModel> GetJobInfo(string jobId);
    }
}
