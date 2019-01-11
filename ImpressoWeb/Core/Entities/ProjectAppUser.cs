namespace Core.Entities
{
    public class ProjectAppUser
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}