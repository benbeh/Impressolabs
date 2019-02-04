using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AppUserEducation
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int EducationId { get; set; }
        public Education Education { get; set; }
    }
}
