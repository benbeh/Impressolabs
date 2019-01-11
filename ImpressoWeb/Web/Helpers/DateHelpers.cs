using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Helpers
{
    public static class DateHelpers
    {
        public static string ToISOString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
        }
    }
}
