using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Enum
{
    public enum SubReasonConnectEnum
    {
        None = 0,
        [Description("Profile / Cv Review")]
        ProfileCvReview = 1,
        [Description("Job Advice")]
        JobAdvice = 2
    }
}
