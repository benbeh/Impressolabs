using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BLL.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(this System.Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static List<string> GetDescriptions(Type value)
        {
            var enums = System.Enum.GetValues(value);
            var result = new List<string>();
            foreach (System.Enum enumValue in enums)
            {
                result.Add(enumValue.GetDescription());
            }
            return result;
        }
    }
}
