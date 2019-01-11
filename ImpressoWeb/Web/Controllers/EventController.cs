using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure;

namespace Web.Controllers
{
    public class EventController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEventService _eventService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EventController(UserManager<AppUser> userManager, IEventService eventService, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _eventService = eventService;
            _hostingEnvironment = environment;
        }


        public IActionResult Index()
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;
            return View(_eventService.GetListEventsByHiringManagerId(userId));
        }

        [HttpGet]
        public ViewResult Create() => View();
        [HttpPost]
        public IActionResult Create(CreateEventViewModel model)
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;
            if (ModelState.IsValid)
            {
                _eventService.CreateEvent(userId, model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ViewResult Edit(int id)
        {
            return View(_eventService.GetEventById(id));
        }
        [HttpPost]
        public IActionResult Edit(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                // save image to folder
                string filePath = "";
                if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                {
                    filePath = "/images/Events/" + model.Id + ".png";

                    using (var stream = new FileStream(_hostingEnvironment.WebRootPath + filePath, FileMode.Create))
                    {
                        model.PhotoFile.CopyTo(stream);
                    }
                }
                // update
                model.PictureSource = filePath;
                _eventService.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _eventService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EventDetails(int eventId)
        {
            var currentEvent = _eventService.GetEventById(eventId);
            return View(currentEvent);
        }

      
    }
}