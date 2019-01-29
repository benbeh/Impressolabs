using System;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Models.Event;

namespace ImpressoApp.Services.Event
{
    public class EventService : IEventService
    {
        private static readonly string SetAsInterestedEndpoint = ApplicationConstants.LiveServerApi + "api/Event/SetAsInterested";


        private readonly IRequestProvider requestProvider;

        public EventService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<BaseResponseModel> SetAsInterestedAsync(string eventId)
        {
            return await requestProvider.PostAsync<BaseResponseModel>(SetAsInterestedEndpoint, 
                                                                      new SetAsInterestedRequestModel { EventId = eventId, IsInterested = true });
        }
    }
}
