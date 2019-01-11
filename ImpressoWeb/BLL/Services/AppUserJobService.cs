using BLL.Interfaces;
using BLL.ViewModels;
using DAL.Interfaces;
using Core.Entities;

namespace BLL.Services
{
    public class AppUserJobService : Service<AppUserJob, AppUserJobViewModel>, IAppUserJobService
    {
        public AppUserJobService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.AppUserJobs)
        {

        }
    }
}