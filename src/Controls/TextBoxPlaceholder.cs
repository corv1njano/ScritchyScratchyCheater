using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Controls
{
    /// <summary>
    /// Represents a TextBox control that displays placeholder text when the input is empty.
    /// </summary>
    public class TextBoxPlaceholder : TextBox
    {
        /// <summary>
        /// Static constructor, because the default style for this custom element can only be
        /// set once. The style cannot be overwritten a second time. The moment this element
        /// is being used inside XAML, an instance of this class is being created. When this
        /// control is used more times, it will get instantiated multiple times, but again, the
        /// metadata can only be overwritten once. So that's why a static constructor is being
        /// used to make sure the class not tries to overwrite the metadata with each new object
        /// of it (this will also never happen, because the app will crash when the metadata has
        /// already been overwritten before).
        /// </summary>
        static TextBoxPlaceholder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextBoxPlaceholder),
                new FrameworkPropertyMetadata(typeof(TextBoxPlaceholder)));
        }

        public TextBoxPlaceholder()
        {
            TextChanged += (_, _) => UpdateHasText();
            UpdateHasText();
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(TextBoxPlaceholder),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.Register(
                nameof(HasText),
                typeof(bool),
                typeof(TextBoxPlaceholder),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsClearableProperty =
            DependencyProperty.Register(
                nameof(IsClearable),
                typeof(bool),
                typeof(TextBoxPlaceholder),
                new PropertyMetadata(false));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public bool HasText
        {
            get => (bool)GetValue(HasTextProperty);
            private set => SetValue(HasTextProperty, value);
        }

        public bool IsClearable
        {
            get => (bool)GetValue(IsClearableProperty);
            set => SetValue(IsClearableProperty, value);
        }

        private void UpdateHasText()
        {
            HasText = !string.IsNullOrEmpty(Text);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_ClearButton") is Button btn)
            {
                btn.Click += (_, _) =>
                {
                    Text = string.Empty;
                    Focus();
                };
            }
        }
    }
}