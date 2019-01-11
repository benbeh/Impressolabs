using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ReferralsController : Controller
    {
        private readonly ITestimonialService _testimonialService;

        public ReferralsController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public IActionResult Index()
        {
            var result = _testimonialService.GetTestimonialsLeftByUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _testimonialService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}