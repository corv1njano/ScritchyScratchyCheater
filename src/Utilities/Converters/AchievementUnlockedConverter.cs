using System.Globalization;
using System.Windows.Data;

namespace ScritchyScratchyCheater.Utilities.Converters
{
    internal class AchievementUnlockedConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool isUnlocked || parameter is not string id) return null;

            return isUnlocked
                ? App.ResourceParser.GetSprite(id)
                : App.ResourceParser.GetSprite(id + "u");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
