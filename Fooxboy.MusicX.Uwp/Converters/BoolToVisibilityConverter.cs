using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Fooxboy.MusicX.Uwp.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var val = (Visibility) value;
            return val == Visibility.Visible;
        }
    }
}