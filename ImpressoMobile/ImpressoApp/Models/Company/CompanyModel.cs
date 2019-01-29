using System;
using System.Collections.Generic;
using ImpressoApp.Models.Job;
using ImpressoApp.Enums;

namespace ImpressoApp.Models.Company
{
    public class CompanyModel : BaseResponseModel
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoSource { get; set; }
        public Industry CompanyArea { get; set; }
        public string Location { get; set; }
        public int EmployeesCount { get; set; }
        public string WhoWeAreText { get; set; }
        public List<JobModel> Vacancies { get; set; }
    }
}
