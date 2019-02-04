using System;
using System.ComponentModel;

namespace ImpressoApp.Models.User
{
    public enum JobType
    {
        [Description ("None")]
        None = 0,

        [Description("Freelancer")]
        Freelancer = 1,

        [Description("Contractor")]
        Contractor = 2
    }

}
