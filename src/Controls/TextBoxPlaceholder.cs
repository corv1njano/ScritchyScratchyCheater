using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Controls
{
    /// <summary>
    /// Represents a TextBox control that displays placeholder text when the input is empty.
    /// </summary>
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