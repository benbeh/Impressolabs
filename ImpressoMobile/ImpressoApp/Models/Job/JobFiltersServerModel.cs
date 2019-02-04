using System;
using System.Collections.Generic;
namespace ImpressoApp.Models.Job
{
    public class JobFiltersServerModel
    {
        public List<string> Companies { get; set; }
        public List<string> JobTypes { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Experience { get; set; }
        public List<string> Industries { get; set; }
        public List<string> Certificates { get; set; }
    }
}
