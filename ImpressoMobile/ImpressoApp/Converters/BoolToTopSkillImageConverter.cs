using System;
using System.Globalization;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class BoolToTopSkillImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool isTop)
            {
                return isTop ? "StarColor.png" : "StarOutline.png";
            }

            return "StarOutline.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
