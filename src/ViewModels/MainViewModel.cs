using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Models.Results;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Dialogs;
using ScritchyScratchyCheater.Views.Pages;
using System.Windows.Controls;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isDragging;

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
        private async Task LoadDraggedFile(string? filePath)
        {
            await SaveFileHelper.LoadSaveFile(filePath);
        }

        [RelayCommand]
        private void OpenAboutApp()
        {
            var dialog = new AboutAppDialog()
            {
                Owner = App.Current.MainWindow
            };
            dialog.ShowDialog();
        }

        [RelayCommand]
        private void OpenSupportMe()
        {
            Utils.OpenUrl("https://github.com/sponsors/corv1njano");
        }
    }
}
