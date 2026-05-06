using System.Windows;

namespace ScritchyScratchyCheater.Controls
{
    /// <summary>
    /// Helper class to fix System.Windows.Data Error 2 and 4 (Binding error for the icon).
    /// Makes the icon a freezable object, which is then being binded.
    /// </summary>
    internal sealed class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
