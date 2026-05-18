using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Controls
{
    public class ButtonWithIcon : Button
    {
        static ButtonWithIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ButtonWithIcon),
                new FrameworkPropertyMetadata(typeof(ButtonWithIcon)));
        }

        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ButtonWithIcon));
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ButtonWithIcon));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // FontSize (optional für Text)
        public static readonly DependencyProperty ButtonWithIconFontSizeProperty =
            DependencyProperty.Register(nameof(ButtonWithIconFontSize), typeof(double), typeof(ButtonWithIcon), new PropertyMetadata(14.0));
        public double ButtonWithIconFontSize
        {
            get => (double)GetValue(ButtonWithIconFontSizeProperty);
            set => SetValue(ButtonWithIconFontSizeProperty, value);
        }

        // IconHeight
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(ButtonWithIcon), new PropertyMetadata(20.0));
        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        // IconWidth
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(ButtonWithIcon), new PropertyMetadata(20.0));
        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        // CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ButtonWithIcon), new PropertyMetadata(new CornerRadius(3)));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
