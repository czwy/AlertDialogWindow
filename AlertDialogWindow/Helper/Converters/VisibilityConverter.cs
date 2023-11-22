using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AlertDialogWindow.Helper
{
    public class VisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            if (parameter == null)
                return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
            else//取反
                return System.Convert.ToBoolean(value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
