using System;
using System.Globalization;
using Android.Webkit;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class StringLenghtToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string text)
            {
                return text.Length > 0 ? false : true;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
