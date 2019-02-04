using System;
using System.Globalization;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class BoolToTagImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = "tag.png";

            if(value is bool tagged)
            {
                source = tagged ? "tagColor.png" : "tag.png";
            }

            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
