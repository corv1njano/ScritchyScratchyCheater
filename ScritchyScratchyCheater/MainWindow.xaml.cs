using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.ViewModels;
using System.Windows;

namespace ScritchyScratchyCheater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowWrapper _windowWrapper;

        public MainWindow()
        {
            InitializeComponent();
            _windowWrapper = new(this);
            DataContext = new MainViewModel();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            _windowWrapper.MinimizeWindow();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            _windowWrapper.CloseWindow();
        }

        private void Button_Maximize(object sender, RoutedEventArgs e)
        {
            _windowWrapper.MaximizeOrRestoreWindow();
        }
    }
}