using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.ViewModels.Dialogs;
using System.Windows;

namespace ScritchyScratchyCheater.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateBackupDialog.xaml
    /// </summary>
    public partial class CreateBackupDialog : Window
    {
        private WindowWrapper _windowWrapper;

        public CreateBackupDialog()
        {
            InitializeComponent();
            _windowWrapper = new WindowWrapper(this);
            DataContext = new CreateBackupViewModel();

            ContentRendered += CreateBackupDialog_ContentRendered;
        }

        private void CreateBackupDialog_ContentRendered(object? sender, EventArgs e)
        {
            if (Owner != null)
            {
                Left = Owner.Left + (Owner.Width - ActualWidth) / 2;
                Top = Owner.Top + (Owner.Height - ActualHeight) / 2;
            }
            ContentRendered -= CreateBackupDialog_ContentRendered;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            _windowWrapper.CloseWindow();
        }
    }
}
