namespace Core.Entities
{
    public class AppUserSkill
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public bool IsTop { get; set; }
    }
}