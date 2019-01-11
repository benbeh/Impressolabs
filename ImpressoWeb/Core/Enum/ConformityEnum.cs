using System.ComponentModel;

namespace Core.Enum
{
    public enum ConformityEnum
    {
        None = 0,
        [Description("Most relevant")]
        MostRelevant = 1,
        [Description("Most recent")]
        MostRecent = 2
    }
}