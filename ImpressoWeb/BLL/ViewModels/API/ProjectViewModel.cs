using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels.API
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int AmountOfCandidates { get; set; }
        public int CompanyId { get; set; }

        public List<ProjectAppUserViewModel> ProjectAppUsers { get; set; }
        public List<JobViewModel> Jobs { get; set; }
    }
}
