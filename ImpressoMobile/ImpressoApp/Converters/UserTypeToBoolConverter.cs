using System;
using System.Globalization;
using Xamarin.Forms;
using ImpressoApp.Enums;
namespace ImpressoApp.Converters
{
    public class UserTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (UserType)value;

            return input == UserType.Business;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = (bool)value;

            return output ? UserType.Business : UserType.Person;
        }
    }
}
