using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Pages;
using System.Windows;
using System.Windows.Controls;

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

            App.SaveFileService.FilePathChanged += OnFilePathChanged;
            App.SaveFileService.SaveFileChanged += OnSaveFileChanged;
            App.PageNavigator.CurrentPageChanged += OnCurrentPageChanged;

            PageContainer.Navigate(new StartPage());
        }

        private void OnFilePathChanged(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                SelectedSaveFilePath.Text = "No save file selected.";
                SelectedSaveFilePath.ToolTip = null;
            }
            else
            {
                SelectedSaveFilePath.Text = path;
                SelectedSaveFilePath.ToolTip = new ToolTip
                {
                    Content = $"Full Path: '{path}'"
                };
            }
        }

        private void OnSaveFileChanged(ISaveFile? saveFile) { }

        private void OnCurrentPageChanged(Page page)
        {
            PageContainer.Navigate(page);
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            _windowWrapper.MinimizeWindow();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            //if (App.SaveFileService.LoadedSaveFile != null)
            //{
            //    var result = MessageBox.Show("A save file is loaded. Are you sure you want to quit? Any unsaved changes will be lost",
            //        "Warning", MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        _windowWrapper.CloseWindow();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            _windowWrapper.CloseWindow();
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Version 1.1.0 (release April 2026)\n\n" +
                "Made by corv1njano. Check out my GitHub:\n" +
                "https://github.com/corv1njano",
                "About this App",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
}