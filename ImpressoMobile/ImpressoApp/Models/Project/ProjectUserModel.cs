using System;
namespace ImpressoApp.Models.Project
{
    public class ProjectUserModel
    {
        public int ProjectId { get; set; }
        public string AppUserId { get; set; }
        public ProjectUserInfoModel AppUser { get; set; }
    }
}
