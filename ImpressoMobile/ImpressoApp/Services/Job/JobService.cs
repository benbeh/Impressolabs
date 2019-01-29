using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models.Job;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Models;
using Newtonsoft.Json.Serialization;
using ImpressoApp.Models.User;
namespace ImpressoApp.Services.Job
{
    public class JobService : IJobService
    {
        private const string JobListEndpoint = ApplicationConstants.LiveServerApi + "api/Job/ListJobs/";
        private const string JobFilterEndpoint = ApplicationConstants.LiveServerApi + "/api/Job/ListSortedJobs/";
        private const string ApplyJobEndpoint = ApplicationConstants.LiveServerApi + "api/Job/ApplyForJob/";
        private const string SetAsBookmarkedEndpoint = ApplicationConstants.LiveServerApi + "/api/Job/SetAsBookmarked/";
        private const string AppliedJobsOfCurrentUserEndpoint = ApplicationConstants.LiveServerApi + "/api/Job/AppliedJobsOfCurrentUser/";
        private const string FiltersEndpoint = ApplicationConstants.LiveServerApi + "/api/Job/GetFilters/";
        private const string CreateJobEndpoint = ApplicationConstants.LiveServerApi + "/api/Job/CreateJob/";
        private const string GetJobInfoEndpoint = ApplicationConstants.LiveServerApi + "/api/Job/GetJobInfo";

        private readonly IRequestProvider requestProvider;

        public JobService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<JobModel>> GetJobs()
        {
            return await requestProvider.GetAsync<List<JobModel>>(JobListEndpoint);
        }

        public async Task CreateJob(JobModel jobModel)
        {
            await requestProvider.PostAsync<BaseResponseModel>(CreateJobEndpoint, jobModel);
        }

        public async Task<List<JobModel>> GetFilteredJobs(JobFilterModel filterModel)
        {
            var filters = new List<ReqestParameter>();

            if (filterModel.Location != null)
            {
                filters.Add(new ReqestParameter { Name = "Location", Value = filterModel.Location });
            }

            if (filterModel.JobTypes != null)
            {
                filters.Add(new ReqestParameter { Name = $"JobTypes", Values = filterModel.JobTypes });
            }

            if (filterModel.CompanyName != null)
            {
                filters.Add(new ReqestParameter { Name = "CompanyName", Value = filterModel.CompanyName });
            }

            if (filterModel.Skills != null)
            {
                filters.Add(new ReqestParameter { Name = "Skills", Values = filterModel.Skills });
            }

            if (filterModel.Experience != null)
            {
                filters.Add(new ReqestParameter { Name = "Experience", Values = filterModel.Experience });
            }

            if (filterModel.Industry != null)
            {
                filters.Add(new ReqestParameter { Name = "Industry", Value = filterModel.Industry });
            }

            if (filterModel.Certificates != null)
            {
                filters.Add(new ReqestParameter { Name = "Certificates[]", Values = filterModel.Certificates });
            }

            var jobs = await requestProvider.GetAsync<List<JobModel>>(JobFilterEndpoint, filters);
#if DEBUG
            foreach (var job in jobs)
            {
                //                job.CompanyLogoSource = "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eHBqYjiuvWxDY+3UyMfv+vlLF151192x2OjEn/7hnX2/yvb/A6x2zv2QSIpbAAAAV3RFWHRjb21tZW50AEZpbGUgc291cmNlOiBodHRwczovL2NvbW1vbnMud2lraW1lZGlhLm9yZy93aWtpL0ZpbGU6RmFjZWJvb2tfbG9nb18oc3F1YXJlKS5wbmcNCxLhAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE1LTA3LTA3VDE4OjI1OjA0KzAwOjAwCzG/vAAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxNS0wNy0wN1QxODoyNTowNCswMDowMHpsBwAAAABGdEVYdHNvZnR3YXJlAEltYWdlTWFnaWNrIDYuNi45LTcgMjAxNC0wMy0wNiBRMTYgaHR0cDovL3d3dy5pbWFnZW1hZ2ljay5vcmeB07PDAAAAGHRFWHRUaHVtYjo6RG9jdW1lbnQ6OlBhZ2VzADGn/7svAAAAGHRFWHRUaHVtYjo6SW1hZ2U6OmhlaWdodAA5NjBohV7eAAAAF3RFWHRUaHVtYjo6SW1hZ2U6OldpZHRoADk2MLQpDVMAAAAZdEVYdFRodW1iOjpNaW1ldHlwZQBpbWFnZS9wbmc/slZOAAAAF3RFWHRUaHVtYjo6TVRpbWUAMTQzNjI5MzUwNJJtD1oAAAATdEVYdFRodW1iOjpTaXplADQzLjZLQkJjqshsAAAAM3RFWHRUaHVtYjo6VVJJAGZpbGU6Ly8vdG1wL2xvY2FsY29weV81NzBjZTZhZmZjYjEtMS5wbme7tnWwAAAAAElFTkSuQmCC";
            }
#endif

            return jobs;
        }

        public async Task<JobFiltersServerModel> GetFilters()
        {
            return await requestProvider.GetAsync<JobFiltersServerModel>(FiltersEndpoint);
        }

        public async Task<BaseResponseModel> ApplyForJobAsync(int jobId)
        {
            var smallModel = new SmallJobModel { Id = jobId.ToString() };
            return await requestProvider.PostAsync<BaseResponseModel>(ApplyJobEndpoint, smallModel);
        }

        public Task<BaseResponseModel> SetAsBookmarkedAsync(SetAsBookmarkedRequestModel model)
        {
            return requestProvider.PostAsync<BaseResponseModel>(SetAsBookmarkedEndpoint, model);
        }

        public async Task<List<JobModel>> GetAppliedJobsAsync()
        {
            return await requestProvider.PostAsync<List<JobModel>>(AppliedJobsOfCurrentUserEndpoint, null);
        }

        public async Task<JobModel> GetJobInfo(string jobId)
        {
            return await requestProvider.GetAsync<JobModel>(GetJobInfoEndpoint, new List<ReqestParameter> { new ReqestParameter() { Name = "Id", Value = jobId } });
        }

        //public Task<List<JobModel>> GetJobs()
        //{
        //    return Task.Run(() =>
        //    {
        //        var job1 = new JobModel
        //        {
        //            Title = "Graphic Designer",
        //            Location = "Cupertino",
        //            Level = "Intemediate",
        //            CompanyName = "Apple Inc",
        //            CompanyLogoSource = "https://banner2.kisspng.com/20171218/f41/apple-logo-png-5a37e212dfda18.3311147015136117949169.jpg",
        //            PostedTime = DateTime.Now,
        //            ApplicantsCount = 17,
        //            IsBookmarked = true,
        //            Description = "We are hiring for a graphic designer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //            JobText = "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //                + "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //                + "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //                + "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //        };

        //        var job2 = new JobModel
        //        {
        //            Title = "iOS Developer",
        //            Location = "Colorado",
        //            Level = "Senior",
        //            CompanyName = "Facebook",
        //            CompanyLogoSource = "https://banner2.kisspng.com/20171216/213/facebook-logo-png-5a35528eaa4f08.7998622015134439826976.jpg",
        //            PostedTime = DateTime.Now,
        //            ApplicantsCount = 15,
        //            Description = "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //            JobText = "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //                + "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //                + "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
        //                + "We are hiring for a iOS developer who is... Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."

        //        };

        //        var resultList = new List<JobModel>();
        //        resultList.Add(job1);
        //        resultList.Add(job2);

        //        return resultList;
        //    });
        //}
    }
}
