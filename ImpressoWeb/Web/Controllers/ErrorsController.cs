using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult AppError500()
        {
            return View();
        }

        public IActionResult AppError404()
        {
            return View();
        }
    }
}
