using System;
using System.Windows.Data;
using System.Globalization;
using Roles;

namespace _4lab.Converters
{
    public class RoleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TeamRole role))
                return string.Empty;

            if (role == TeamRole.Captain)
                return "Captain";
            if (role == TeamRole.Member)
                return "Member";
            if (role == TeamRole.Support)
                return "Support";

            return role.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}