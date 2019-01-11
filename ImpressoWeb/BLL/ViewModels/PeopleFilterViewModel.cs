using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels
{
    public class PeopleFilterViewModel
    {
        public string Location { get; set; }
        public string Industry { get; set; }
        public List<string> JobTypes { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Experience { get; set; }
        public List<string> Certificates { get; set; }
        public string CompanyName { get; set; }
    }
}
