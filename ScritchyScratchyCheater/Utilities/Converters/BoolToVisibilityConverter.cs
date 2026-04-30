using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ScritchyScratchyCheater.Utilities.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public bool UseHidden { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool boolValue) return Visibility.Collapsed;

            if (Invert) boolValue = !boolValue;

            if (boolValue) return Visibility.Visible;

            return UseHidden ? Visibility.Hidden : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility vis)
            {
                bool result = vis == Visibility.Visible;
                return Invert ? !result : result;
            }

            return false;
        }
    }
}
