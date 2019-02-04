using BLL.AutoMapper;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using DAL.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Enum;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Services
{
    public class EventService : Service<Event, EventViewModel>, IEventService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public EventService(IUnitOfWork unitOfWork, IHostingEnvironment environment) : base(unitOfWork, unitOfWork.Events)
        {
            _hostingEnvironment = environment;
        }

        public void CreateEvent(string userId, CreateEventViewModel model)
        {
            var companyAppUser = Database.CompanyAppUsers.GetAll().First(companyUser => companyUser.AppUserId == userId);

            Event createdEvent = new Event()
            {
                Title = model.Title,
                Description = model.Description,
                Country = model.Country,
                City = model.City,
                Address = model.Address,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PrivacyType = (PrivacyTypeEnum)model.PrivacyType,
                CompanyId = companyAppUser.CompanyId
            };

            var resultEvent = Database.Events.Add(createdEvent);
            Database.Save();

            // save image to folder
            string filePath = "";
            if (model.PhotoFile != null && model.PhotoFile.Length > 0)
            {
                filePath = "/images/Events/" + resultEvent.Id + ".png";

                using (var stream = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                {
                    model.PhotoFile.CopyTo(stream);
                }
            }
            // update
            resultEvent.PictureSource = filePath;
            Database.Events.Update(resultEvent);
            Database.Save();
        }

        public IEnumerable<EventViewModel> GetListEventsByUserId(string userId)
        {
            var eventsList = Mapping.Map<IEnumerable<Event>, IEnumerable<EventViewModel>>(Database.Events.GetAll().Include(events => events.Company), opts => opts.Items["CurrentUserId"] = userId);
            foreach(var item in eventsList)
            {
                item.IsBookmarked = Database.InterestedEvents.GetAll().Any(bookmarkedEvent => bookmarkedEvent.AppUserId == userId && bookmarkedEvent.EventId == item.Id);
            }
            return eventsList;
        }

        public IEnumerable<EventViewModel> GetBookmakedEventsByUserId(string userId)
        {
            var bookmarkedEvents = Database.InterestedEvents.GetAll().Where(bookmarkedEvent => bookmarkedEvent.AppUserId == userId).Include(bookmarkedEvent => bookmarkedEvent.Event).Select(eventViewModel => new EventViewModel {
                Title = eventViewModel.Event.Title,
                Description = eventViewModel.Event.Description,
                StartDate = eventViewModel.Event.StartDate,
                EndDate = eventViewModel.Event.EndDate,
                Country = eventViewModel.Event.Country,
                City = eventViewModel.Event.City,
                Address = eventViewModel.Event.Address,
                PictureSource = eventViewModel.Event.PictureSource,
                PrivacyType = eventViewModel.Event.PrivacyType,
                HostedByName = eventViewModel.Event.Company.Name,
            }).ToList();
            return bookmarkedEvents;
        }

        public IEnumerable<EventViewModel> GetListEventsByHiringManagerId(string managerId)
        {
            var companyId = Database.CompanyAppUsers.GetAll().Include(companyUser => companyUser.Company).Where(companyUser => companyUser.AppUserId == managerId).Select(companyUser => companyUser.CompanyId).FirstOrDefault();
            var events = Database.Events.GetAll().Include(companyEvents => companyEvents.Company).Where(companyEvents => companyEvents.CompanyId == companyId);
            var eventsList = Mapping.Map<IEnumerable<Event>, IEnumerable<EventViewModel>>(events);

            return eventsList;
        }

        public EventViewModel GetEventById(int eventId)
        {
            Event currentEvent = Database.Events.GetAll().Include(e => e.Company).First(e => e.Id == eventId);
            return Mapping.Map<Event, EventViewModel>(currentEvent);

        }
    }
}
