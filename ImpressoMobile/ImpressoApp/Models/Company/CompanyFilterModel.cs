using System;
using ImpressoApp.Enums;
namespace ImpressoApp.Models.Company
{
    public class CompanyFilterModel
    {
        public string Location { get; set; }

        public JobType JobType { get; set; } = JobType.Contractor;

        public Industry Industry { get; set; } = Industry.None;

        public int Size { get; set; }
    }
}
