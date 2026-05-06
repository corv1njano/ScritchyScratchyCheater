using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Controls
{
    public class BetterButton : Button
    {
        static BetterButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BetterButton),
                new FrameworkPropertyMetadata(typeof(BetterButton)));
        }

        #region icon / text toggle
        public static readonly DependencyProperty HasIconProperty =
            DependencyProperty.Register(nameof(HasIcon), typeof(bool), typeof(BetterButton), new PropertyMetadata(true));
        public bool HasIcon
        {
            get => (bool)GetValue(HasIconProperty);
            set => SetValue(HasIconProperty, value);
        }

        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register(nameof(HasText), typeof(bool), typeof(BetterButton), new PropertyMetadata(true));
        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            set => SetValue(HasTextProperty, value);
        }
        #endregion

        // Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(BetterButton));
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(BetterButton));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // FontSize (optional für Text)
        public static readonly DependencyProperty BetterButtonFontSizeProperty =
            DependencyProperty.Register(nameof(BetterButtonFontSize), typeof(double), typeof(BetterButton), new PropertyMetadata(14.0));
        public double BetterButtonFontSize
        {
            get => (double)GetValue(BetterButtonFontSizeProperty);
            set => SetValue(BetterButtonFontSizeProperty, value);
        }

        // IconHeight
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(BetterButton), new PropertyMetadata(20.0));
        public double IconHeight
        {
            get => (double)GetValue(IconHeightProperty);
            set => SetValue(IconHeightProperty, value);
        }

        // IconWidth
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(BetterButton), new PropertyMetadata(20.0));
        public double IconWidth
        {
            get => (double)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        // Text Alignment
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(TextHorizontalAlignment), typeof(HorizontalAlignment), typeof(BetterButton),
                new PropertyMetadata(HorizontalAlignment.Center));
        public HorizontalAlignment TextHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty);
            set => SetValue(TextHorizontalAlignmentProperty, value);
        }

        // Foreground Color
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("Font.Secondary")));
        public Brush ForegroundColor
        {
            get => (Brush)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        // Foreground Hover Color
        public static readonly DependencyProperty ForegroundHoverColorProperty =
            DependencyProperty.Register(nameof(ForegroundHoverColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("Font.Secondary.Hover")));
        public Brush ForegroundHoverColor
        {
            get => (Brush)GetValue(ForegroundHoverColorProperty);
            set => SetValue(ForegroundHoverColorProperty, value);
        }

        // Disabled Foreground Color
        public static readonly DependencyProperty DisabledForegroundColorProperty =
            DependencyProperty.Register(nameof(DisabledForegroundColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Text.Disabled")));
        public Brush DisabledForegroundColor
        {
            get => (Brush)GetValue(DisabledForegroundColorProperty);
            set => SetValue(DisabledForegroundColorProperty, value);
        }

        // Background Color
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background")));
        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        // Background Hover Color
        public static readonly DependencyProperty BackgroundHoverColorProperty =
            DependencyProperty.Register(nameof(BackgroundHoverColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background.Hover")));
        public Brush BackgroundHoverColor
        {
            get => (Brush)GetValue(BackgroundHoverColorProperty);
            set => SetValue(BackgroundHoverColorProperty, value);
        }

        // Disabled Background Color
        public static readonly DependencyProperty DisabledBackgroundColorProperty =
            DependencyProperty.Register(nameof(DisabledBackgroundColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background.Disabled")));
        public Brush DisabledBackgroundColor
        {
            get => (Brush)GetValue(DisabledBackgroundColorProperty);
            set => SetValue(DisabledBackgroundColorProperty, value);
        }

        // Border Color
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border")));
        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        // Border Hover Color
        public static readonly DependencyProperty BorderHoverColorProperty =
            DependencyProperty.Register(nameof(BorderHoverColor), typeof(Brush), typeof(BetterButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Hover")));
        public Brush BorderHoverColor
        {
            get => (Brush)GetValue(BorderHoverColorProperty);
            set => SetValue(BorderHoverColorProperty, value);
        }

        // Border Thickness
        public static readonly DependencyProperty BorderSizeProperty =
            DependencyProperty.Register(nameof(BorderSize), typeof(Thickness), typeof(BetterButton),
                new PropertyMetadata(new Thickness(1)));
        public Thickness BorderSize
        {
            get => (Thickness)GetValue(BorderSizeProperty);
            set => SetValue(BorderSizeProperty, value);
        }

        // CornerRadius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(BetterButton),
                new PropertyMetadata(new CornerRadius(4)));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
