using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Enum
{
    public enum RoleEnum
    {
        None = 0,
        User = 1,
        [Description("Hiring manager")]
        HiringManager = 2,
        Admin = 3
    }
}

