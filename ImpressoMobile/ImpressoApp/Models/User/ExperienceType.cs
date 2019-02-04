using System;
using System.ComponentModel;

namespace ImpressoApp.Models.User
{
    public enum ExperienceType
    {
        [Description("No experience")]
        None = 0,

        [Description("Beginner")]
        Beginner = 1,

        [Description("Intermediate")]
        Intermediate = 2,

        [Description("Advanced")]
        Edvanced = 3
    }
}
