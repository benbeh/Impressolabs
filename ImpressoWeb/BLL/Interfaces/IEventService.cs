using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IEventService : IService<Event, EventViewModel>
    {
        IEnumerable<EventViewModel> GetListEventsByUserId(string userId);

        void CreateEvent(string userId, CreateEventViewModel model);

        IEnumerable<EventViewModel> GetBookmakedEventsByUserId(string userId);

        IEnumerable<EventViewModel> GetListEventsByHiringManagerId(string managerId);

        EventViewModel GetEventById(int eventId);

    }
}
