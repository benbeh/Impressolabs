using System;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.ObjectModel;
namespace ImpressoApp.Converters
{
    public class IdToBoolConverterMultiple : IValueConverter
    {
        public IdToBoolConverterMultiple()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string id)
            {
                var source = ((ListView)parameter)?.ItemsSource;
                return (source as ObservableCollection<string>)?.Contains(id);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
