using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Enum
{
    public enum TypeOfWorkEnum
    {
        None = 0,
        [Description("Part Time")]
        PartTime = 1,
        [Description("Full Time")]
        FullTime = 2
    }
}
