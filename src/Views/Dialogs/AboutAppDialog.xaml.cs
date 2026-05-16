using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.ViewModels.Dialogs;
using System.Windows;

namespace ScritchyScratchyCheater.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutAppDialog.xaml
    /// </summary>
    public partial class AboutAppDialog : Window
    {
        private WindowWrapper _windowWrapper;

        public AboutAppDialog()
        {
            InitializeComponent();
            _windowWrapper = new WindowWrapper(this);
            DataContext = new AboutAppViewModel();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            _windowWrapper.CloseWindow();
        }
    }
}
