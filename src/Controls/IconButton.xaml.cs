using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Controls
{
    public partial class IconButton : Button
    {
        public IconButton()
        {
            InitializeComponent();
        }

        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(IconButton));

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconButton));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // FontSize (optional für Text)
        public static readonly DependencyProperty IconButtonFontSizeProperty =
            DependencyProperty.Register(nameof(IconButtonFontSize), typeof(double), typeof(IconButton), new PropertyMetadata(14.0));

        public double IconButtonFontSize
        {
            get => (double)GetValue(IconButtonFontSizeProperty);
            set => SetValue(IconButtonFontSizeProperty, value);
        }

        // IconHeight
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(IconButton), new PropertyMetadata(20.0));

        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        // IconWidth
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(IconButton), new PropertyMetadata(20.0));

        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        // CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(IconButton), new PropertyMetadata(new CornerRadius(4)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
