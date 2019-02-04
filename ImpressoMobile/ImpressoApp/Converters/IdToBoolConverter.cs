using System;
using System.Globalization;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class IdToBoolConverter : IValueConverter
    {
        public IdToBoolConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string id)
            {
                return id == ((Label)parameter).Text;
            }

            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
