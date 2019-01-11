using System.Collections.Generic;
using System.Linq;
using BLL.ViewModels.API;
using Core.Enum;

namespace BLL.ViewModels
{
    public class ConnectPersonCompanyViewModel
    {
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IndustryEnum Industry { get; set; }

        public IEnumerable<JobViewModel> Jobs { get; set; }
    }
}