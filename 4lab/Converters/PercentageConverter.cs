using System;
using System.Globalization;
using System.Windows.Data;

namespace _4lab
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double size && parameter is string percentString && double.TryParse(percentString, NumberStyles.Any, CultureInfo.InvariantCulture, out double percent))
            {
                return size * percent;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}