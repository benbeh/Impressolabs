using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels
{
    public class CompanyFilterViewModel
    {
        public string Location { get; set; }
        public string Industry { get; set; }
        public int MinCompanySize { get; set; }
        public int MaxCompanySize { get; set; }
    }
}
