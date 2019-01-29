using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models;
using ImpressoApp.Models.Profile;
using ImpressoApp.Models.User;
using ImpressoApp.Services.RequestProvider;

namespace ImpressoApp.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private const string ListTagsEndpoint = ApplicationConstants.LiveServerApi + "/api/Tag/ListTags/";

        private readonly IRequestProvider requestProvider;
        
        public ProfileService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public Task<List<ProfileJobModel>> GetJobsAsync()
        {
            return Task.Run(() =>
            {
                var jobs = new List<ProfileJobModel>();

                var job1 = new ProfileJobModel
                {
                    Position = "Senior Developer",
                    ProjectName = "Project One",
                    PositionsCount = 1,
                    Timestamp = DateTime.Now
                };

                var job2 = new ProfileJobModel
                {
                    Position = "Expert Graphic",
                    ProjectName = "Project One",
                    PositionsCount = 2,
                    Timestamp = DateTime.Now
                };

                var job3 = new ProfileJobModel
                {
                    Position = "Senior Developer",
                    ProjectName = "Project One",
                    PositionsCount = 3,
                    Timestamp = DateTime.Now
                };

                var job4 = new ProfileJobModel
                {
                    Position = "Beginer Tester",
                    ProjectName = "Project One",
                    PositionsCount = 4,
                    Timestamp = DateTime.Now
                };

                var job5 = new ProfileJobModel
                {
                   Position = "Junior Developer",
                   ProjectName = "Project One",
                   PositionsCount = 5,
                   Timestamp = DateTime.Now
                };

                var job6 = new ProfileJobModel
                {
                    Position = "Senior Developer",
                    ProjectName = "Project One",
                    PositionsCount = 6,
                    Timestamp = DateTime.Now
                };

                jobs.Add(job1);
                jobs.Add(job2);
                jobs.Add(job3);
                jobs.Add(job4);
                jobs.Add(job5);
                jobs.Add(job6);

                return jobs;
            });
        }

        public async Task<List<TagServerModel>> GetTagsAsync()
        {
            return await requestProvider.GetAsync<List<TagServerModel>>(ListTagsEndpoint);
        }

        //public Task<List<ProfileTagModel>> GetTagsAsync()
        //{
        //    return Task.Run(() =>
        //    {

        //        var tags = new List<ProfileTagModel>();

        //        var tag1 = new ProfileTagModel
        //        {
        //            Title = "Apple UX designer job",
        //            Role = "Technology ompany",
        //            Timestamp = DateTime.Today
        //        };

        //        var tag2 = new ProfileTagModel
        //        {
        //            Title = "Bobby Cohen",
        //            Role = "iOS Developer",
        //            Timestamp = DateTime.Today
        //        };

        //        var tag3 = new ProfileTagModel
        //        {
        //            Title = "Annie Chanmbers",
        //            Role = "Android developer",
        //            Timestamp = DateTime.Today
        //        };

        //        var tag4 = new ProfileTagModel
        //        {
        //            Title = "Mark Moran",
        //            Role = "UX developer",
        //            Timestamp = DateTime.Today
        //        };

        //        var tag5 = new ProfileTagModel
        //        {
        //            Title = "James Snyder",
        //            Role = "Writer",
        //            Timestamp = DateTime.Today
        //        };

        //        tags.Add(tag1);
        //        tags.Add(tag2);
        //        tags.Add(tag3);
        //        tags.Add(tag4);
        //        tags.Add(tag5);

        //        return tags;
        //    });
        //}
    }
}
