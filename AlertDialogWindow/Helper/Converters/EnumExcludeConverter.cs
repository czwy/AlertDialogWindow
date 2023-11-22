using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AlertDialogWindow.Helper
{
    public class EnumExcludeConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name == "Boolean")
            {
                if (value == null || parameter == null) return true;
                else return !(parameter.ToString().IndexOf(value.ToString()) >= 0);
            }
            else if (targetType.Name == "Visibility")
            {
                if (value == null || parameter == null) return Visibility.Visible;
                else return parameter.ToString().Split('|').Contains(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
