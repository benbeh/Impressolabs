using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Models.Profile;
using ImpressoApp.Models;
using ImpressoApp.Models.User;
namespace ImpressoApp.Services.Profile
{
    public interface IProfileService
    {
        Task<List<TagServerModel>> GetTagsAsync();
        Task<List<ProfileJobModel>> GetJobsAsync();
    }
}
