using System;
using Xamarin.Forms.Xaml.Internals;
using Xamarin.Forms;
using System.Globalization;
using ImpressoApp.Extensions;

namespace ImpressoApp.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is Enum @enum)
                {
                    return @enum.DescriptionAttr();
                }
            }
            catch (Exception)
            {
                return value.ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
