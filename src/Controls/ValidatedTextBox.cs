using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Controls
{
    /// <summary>
    /// Represents a text box control that validates its input and indicates whether the input is valid.
    /// </summary>
    public class ValidatedTextBox : TextBox
    {
        public static readonly DependencyProperty IsValidProperty =
        DependencyProperty.Register(
            nameof(IsValid),
            typeof(bool),
            typeof(ValidatedTextBox),
            new PropertyMetadata(true));

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set => SetValue(IsValidProperty, value);
        }

        static ValidatedTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ValidatedTextBox),
                new FrameworkPropertyMetadata(typeof(ValidatedTextBox)));
        }
    }
}
