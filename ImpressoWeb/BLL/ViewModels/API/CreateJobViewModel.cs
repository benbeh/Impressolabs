
using Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels.API
{
    public class CreateJobViewModel
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public ExperienceEnum Level { get; set; }
        public int ProjectId { get; set; }
        public DateTime PostedTime { get; set; }
        public string Description { get; set; }
        public TypeOfWorkEnum TypeOfWork { get; set; }

        public IEnumerable<string> Skills { get; set; }
    }
}
