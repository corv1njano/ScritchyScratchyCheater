using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ScritchyScratchyCheater.Utilities.Converters
{
    /// <summary>
    /// Provides a value converter that maps Boolean values to WPF Visibility values.
    /// </summary>
    internal class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Inverts the conversion.
        /// </summary>
        public bool Invert { get; set; } = false;

        /// <summary>
        /// Uses <see cref="Visibility.Hidden"/> instead of <see cref="Visibility.Collapsed"/>.
        /// </summary>
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
