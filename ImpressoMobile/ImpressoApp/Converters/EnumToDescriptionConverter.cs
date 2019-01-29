using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using ImpressoApp.Utils;
using ImpressoApp.Extensions;

namespace ImpressoApp.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = new List<string>();

            if (value is IEnumerable enumerable)
            {
                foreach (var obj in enumerable)
                {
                    items.Add(obj.DescriptionAttr());
                }
            }

            return items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
