using System;
using Core.Enum;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BLL.ViewModels.API
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoSource { get; set; }
        public IndustryEnum CompanyArea { get; set; }
        public string Location { get; set; }
        public string WhoWeAreText { get; set; }
        public DateTime LastUpdate { get; set; }

        public IFormFile PhotoFile { get; set; }

        public int EmployeesCount { get; set; }
 
        public IEnumerable<JobViewModel> Vacancies { get; set; }
    }
}