using System;
using System.Globalization;
using Xamarin.Forms;

namespace ImpressoApp.Converters
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double opacity = 1.0;

            if(value is bool val)
            {
                opacity = val ? 1.0 : 0.0;
            }

            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
