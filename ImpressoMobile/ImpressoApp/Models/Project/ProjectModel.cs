using System;
using System.Collections.Generic;
using ImpressoApp.Models.Job;
namespace ImpressoApp.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int AmountOfCandidates { get; set; }
        public int CompanyId { get; set; }
        public List<ProjectUserModel> ProjectAppUsers { get; set; }
        public List<JobModel> Jobs { get; set; }
    }
}
