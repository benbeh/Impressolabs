using System;
using System.Threading.Tasks;
using ImpressoApp.Models;
namespace ImpressoApp.Services.Event
{
    public interface IEventService
    {
        Task<BaseResponseModel> SetAsInterestedAsync(string eventId);
    }
}
