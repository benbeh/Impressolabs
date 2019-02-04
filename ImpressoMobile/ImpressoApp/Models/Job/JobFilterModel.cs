using System;
using System.Collections;
using System.Collections.Generic;

namespace ImpressoApp.Models.Job
{
    public class JobFilterModel
    {
        public bool IsMostRelevant { get; set; }

        public string CompanyName { get; set; }

        public string Location { get; set; }

        public string Industry { get; set; }

        public List<string> Skills { get; set; }

        public List<string> JobTypes { get; set; }

        public List<string> Certificates { get; set; }

        public List<string> Experience { get; set; }

        public int MinCompanySize { get; set; }

        public int MaxCompanySize { get; set; }
    }
}
