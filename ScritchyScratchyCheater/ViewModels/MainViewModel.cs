using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Views.Dialogs;
using ScritchyScratchyCheater.Views.Pages;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserControl? _currentPage = new StartingPage();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SaveFileTooltip))]
        private string _selectedSaveFilePath = "No save file selected.";
        public ToolTip? SaveFileTooltip => string.IsNullOrWhiteSpace(SelectedSaveFilePath)
            || SelectedSaveFilePath == "No save file selected."
            ? null
            : new ToolTip { Content = $"Full Path: '{SelectedSaveFilePath}'" };

        public MainViewModel()
        {
            App.PageNavigator.CurrentPageChanged += HandleCurrentPageChanged;
            App.SaveFileService.FilePathChanged += HandleFilePathChanged;
        }

        private void HandleCurrentPageChanged(UserControl page)
        {
            CurrentPage = page;
        }

        private void HandleFilePathChanged(string filePath)
        {
            SelectedSaveFilePath = string.IsNullOrWhiteSpace(filePath)
                ? "No save file selected."
                : filePath;
        }

        [RelayCommand]
        private void OpenAboutApp()
        {
            //MessageBox.Show(
            //    "Version 1.2.0 (release April 2026)\n\n" +
            //    "Made by corv1njano. Check out my GitHub:\n" +
            //    "https://github.com/corv1njano",
            //    "About this App",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information
            //);

            var dialog = new AboutAppDialog()
            {
                Owner = App.Current.MainWindow
            };
            dialog.ShowDialog();
        }
    }
}
