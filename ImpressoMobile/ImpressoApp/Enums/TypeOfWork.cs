using System;
using System.ComponentModel;

namespace ImpressoApp.Enums
{
    public enum TypeOfWork
    {
        [Description("None")]
        None = 0,
        [Description("Part Time")]
        PartTime = 1,
        [Description("Full Time")]
        FullTime = 2
    }
}
