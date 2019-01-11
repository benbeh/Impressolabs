using System.Collections.Generic;

namespace BLL.ViewModels
{
    public class JobFilterViewModel
    {
        public bool IsMostRelevant { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public List<string> JobTypes { get; set; }
        public List<string> Skills { get; set; }   
        public List<string> Experience { get; set; }
        public string Industry { get; set; }
        public List<string> Certificates { get; set; }
        public int MinCompanySize { get; set; }
        public int MaxCompanySize { get; set; }
    }
}