using System.Collections.Generic;
using BLL.ViewModels.API;
using Core.Enum;

namespace BLL.ViewModels
{
    public class SavePersonForProjectViewModel
    {
        public IEnumerable<ProjectViewModel> Projects { get; set; }
    }
}