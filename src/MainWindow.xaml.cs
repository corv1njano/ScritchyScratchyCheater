using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.ViewModels;
using ScritchyScratchyCheater.Views.Pages;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ScritchyScratchyCheater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowWrapper _windowWrapper;
        private MainViewModel _viewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            _windowWrapper = new(this);
            DataContext = new MainViewModel();

            Loaded += async (_, _) => await _viewModel.CheckForUpdateAsync();
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

        #region drag events
        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (IsDragValid(e)) _viewModel.IsDragging = true;
            //_viewModel.IsDragging = true;
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            _viewModel.IsDragging = false;
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = IsDragValid(e) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            _viewModel.IsDragging = false;

            if (!IsDragValid(e)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var filePath = files.FirstOrDefault();

            _viewModel.LoadDraggedFileCommand.Execute(filePath);
        }

        private bool IsDragValid(DragEventArgs e)
        {
            if (_viewModel.CurrentPage is not StartingPage) return false;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return false;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var filePath = files.FirstOrDefault();

            return filePath != null
                && (Path.GetExtension(filePath).Equals(".json", StringComparison.OrdinalIgnoreCase)
                || Path.GetExtension(filePath).Equals(".backup", StringComparison.OrdinalIgnoreCase));
        }
        #endregion
    }
}