using System;
using System.Globalization;
using Xamarin.Forms;
using FFImageLoading.Forms;
using System.IO;
using ImpressoApp.Constants;

namespace ImpressoApp.Converters
{
    public class Base64ToFFImageSourceConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string source && !string.IsNullOrEmpty(source))
            {
                if (source.Contains("http"))
                {
                    return source;
                }
                else if(source.Contains("/images/"))
                {
                    return $"{ApplicationConstants.LiveServerApi}{source}";
                }
                else
                {
                    return ImageSource.FromStream(
                            () => new MemoryStream(Convert.FromBase64String(source)));
                }
            }

            return "logo.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
