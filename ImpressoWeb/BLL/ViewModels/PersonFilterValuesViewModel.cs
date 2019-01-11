using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels
{
    public class PersonFilterValuesViewModel
    {
        public string Location { get; set; }
        public List<string> Industries { get; set; }
        public List<string> JobTypes { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Experience { get; set; }
        public List<string> Certificates { get; set; }
        public List<string> CompanyNames { get; set; }

        public int JobId { get; set; }
    }
}
