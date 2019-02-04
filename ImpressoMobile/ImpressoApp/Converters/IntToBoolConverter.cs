using System;
using System.Globalization;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (int)value;

            if (input > 0)
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = (bool)value;

            if (output)
                return 1;

            return 0;
        }
    }
}
