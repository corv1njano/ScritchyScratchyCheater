using System.Globalization;
using System.Windows.Data;

namespace ScritchyScratchyCheater.Utilities.Converters
{
    /// <summary>
    /// Converts an icon ID from parsed game data to its corresponding image as a BitmapSource.
    /// </summary>
    internal class IconIdToImageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string id)
            {
                return App.ResourceParser.GetSprite(id);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
