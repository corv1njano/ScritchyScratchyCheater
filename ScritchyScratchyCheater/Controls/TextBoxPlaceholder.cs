using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Controls
{
    public class TextBoxPlaceholder : TextBox
    {

        public TextBoxPlaceholder()
        {
            TextChanged += (_, __) => UpdateHasText();
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

        private void UpdateHasText()
        {
            HasText = !string.IsNullOrEmpty(Text);
        }
    }
}