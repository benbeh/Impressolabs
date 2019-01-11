using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Enum
{
    public enum ReasonConnectEnum
    {
        None = 0,
        [Description("Professional date")]
        ProffecionalDate = 1,
        [Description("Request Assistance")]
        RequestAssistance = 2
    }
}
