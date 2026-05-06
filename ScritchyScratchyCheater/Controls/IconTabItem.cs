using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Controls
{
    /// <summary>
    /// Represents a tab item that displays an icon in addition to its header content.
    /// </summary>
    public class IconTabItem : TabItem
    {
        static IconTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(IconTabItem),
                new FrameworkPropertyMetadata(typeof(IconTabItem)));
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(ImageSource),
                typeof(IconTabItem),
                new PropertyMetadata(null));
    }
}
