using System.ComponentModel;

namespace Core.Enum
{
    public enum JobTypeEnum
    {
        None = 0,
        Freelance = 1,
        Contractor = 2,
        [Description("Permanent Employee")]
        PermanentEmployee = 3,
        [Description("Temporary Employee")]
        TemporaryEmployee = 4,
        Consultant = 5
    }
}