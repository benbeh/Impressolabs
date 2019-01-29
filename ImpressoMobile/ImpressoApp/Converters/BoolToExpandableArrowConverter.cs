using System;
using System.Globalization;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class BoolToExpandableArrowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageResource = "arrowDown.png";

            if(value is bool isExpand)
            {
                imageResource = isExpand ? "arrowUp.png" : "arrowDown.png";
            }

            return imageResource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
