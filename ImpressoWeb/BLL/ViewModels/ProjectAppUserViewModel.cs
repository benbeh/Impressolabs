using BLL.ViewModels.API;

namespace BLL.ViewModels
{
    public class ProjectAppUserViewModel
    {
        public int ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        public string AppUserId { get; set; }
        public AppUserViewModel AppUser { get; set; }
    }
}