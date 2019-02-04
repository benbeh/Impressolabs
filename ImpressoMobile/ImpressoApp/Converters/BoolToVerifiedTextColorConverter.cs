using System;
using System.Globalization;
using Xamarin.Forms;

namespace ImpressoApp.Converters
{
    public class BoolToVerifiedTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool verified)
            {
                return verified ? 
                    (Color)Application.Current.Resources["GrayColor"] : 
                    (Color)Application.Current.Resources["MagentaColor"];
            }

            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
