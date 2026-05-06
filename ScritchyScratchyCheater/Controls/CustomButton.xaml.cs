using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Controls
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : Button
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        // Text Alignment
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(TextHorizontalAlignment), typeof(HorizontalAlignment), typeof(CustomButton),
                new PropertyMetadata(HorizontalAlignment.Center));
        public HorizontalAlignment TextHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty);
            set => SetValue(TextHorizontalAlignmentProperty, value);
        }

        // Foreground Color
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("Font.Secondary")));
        public Brush ForegroundColor
        {
            get => (Brush)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        // Foreground Hover Color
        public static readonly DependencyProperty ForegroundHoverColorProperty =
            DependencyProperty.Register(nameof(ForegroundHoverColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("Font.Secondary.Hover")));
        public Brush ForegroundHoverColor
        {
            get => (Brush)GetValue(ForegroundHoverColorProperty);
            set => SetValue(ForegroundHoverColorProperty, value);
        }

        // Disabled Foreground Color
        public static readonly DependencyProperty DisabledForegroundColorProperty =
            DependencyProperty.Register(nameof(DisabledForegroundColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Text.Disabled")));
        public Brush DisabledForegroundColor
        {
            get => (Brush)GetValue(DisabledForegroundColorProperty);
            set => SetValue(DisabledForegroundColorProperty, value);
        }

        // Background Color
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background")));
        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        // Background Hover Color
        public static readonly DependencyProperty BackgroundHoverColorProperty =
            DependencyProperty.Register(nameof(BackgroundHoverColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background.Hover")));
        public Brush BackgroundHoverColor
        {
            get => (Brush)GetValue(BackgroundHoverColorProperty);
            set => SetValue(BackgroundHoverColorProperty, value);
        }

        // Disabled Background Color
        public static readonly DependencyProperty DisabledBackgroundColorProperty =
            DependencyProperty.Register(nameof(DisabledBackgroundColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background.Disabled")));
        public Brush DisabledBackgroundColor
        {
            get => (Brush)GetValue(DisabledBackgroundColorProperty);
            set => SetValue(DisabledBackgroundColorProperty, value);
        }

        // Border Color
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background")));
        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        // Border Hover Color
        public static readonly DependencyProperty BorderHoverColorProperty =
            DependencyProperty.Register(nameof(BorderHoverColor), typeof(Brush), typeof(CustomButton),
                new PropertyMetadata((SolidColorBrush)Application.Current.TryFindResource("ControlElement.Background")));
        public Brush BorderHoverColor
        {
            get => (Brush)GetValue(BorderHoverColorProperty);
            set => SetValue(BorderHoverColorProperty, value);
        }

        // Border Thickness
        public static readonly DependencyProperty BorderSizeProperty =
            DependencyProperty.Register(nameof(BorderSize), typeof(Thickness), typeof(CustomButton),
                new PropertyMetadata(new Thickness(0)));
        public Thickness BorderSize
        {
            get => (Thickness)GetValue(BorderSizeProperty);
            set => SetValue(BorderSizeProperty, value);
        }

        // Corner Radius
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CustomButton),
                new PropertyMetadata(new CornerRadius(4)));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
