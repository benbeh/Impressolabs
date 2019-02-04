using System;
using System.Globalization;
using Xamarin.Forms;
namespace ImpressoApp.Converters
{
    public class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime time)
            {
                TimeSpan span = DateTime.Now - time;

                if(span.Days > 365)
                {
                    int years = (span.Days / 365);
                    if(span.Days % 365 != 0)
                    {
                        years += 1;
                    }

                    return $"{years} year(s) ago";
                }

                if(span.Days > 30)
                {
                    int months = (span.Days / 30);
                    if (span.Days % 31 != 0)
                    {
                        months += 1;
                    }

                    return $"{months} month(s) ago";
                }

                if(span.Days > 0)
                {
                    return $" {span.Days} day(s) ago";
                }

                if (span.Hours > 0)
                {
                    return $" {span.Hours} hour(s) ago";
                }

                if (span.Minutes > 0)
                {
                    return $" {span.Minutes} minute(s) ago";
                }

                if (span.Seconds > 0)
                {
                    return $" {span.Seconds} second(s) ago";
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
