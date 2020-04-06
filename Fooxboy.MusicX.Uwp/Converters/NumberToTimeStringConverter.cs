using System;
using Windows.UI.Xaml.Data;

namespace Fooxboy.MusicX.Uwp.Converters
{
	public class NumberToTimeStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			switch (value)
			{
				case int _:
				case double _:
				{
					var timeSpan = TimeSpan.FromSeconds(System.Convert.ToDouble(value));
					return timeSpan.Hours > 0 ? $"{timeSpan:h\\:mm\\:ss}" : $"{timeSpan:mm\\:ss}";
				}
				default:
					return string.Empty;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}