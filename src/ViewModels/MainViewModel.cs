using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Dialogs;
using ScritchyScratchyCheater.Views.Pages;
using System.Windows.Controls;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isDragging = false;

        [ObservableProperty]
        private UserControl? _currentPage = new StartingPage();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SaveFileTooltip))]
        private string _selectedSaveFilePath = "No save file selected.";
        public string? SaveFileTooltip => string.IsNullOrWhiteSpace(SelectedSaveFilePath)
            || SelectedSaveFilePath == "No save file selected."
            ? null
            : $"Full Path: '{SelectedSaveFilePath}'";

        public MainViewModel()
        {
            App.PageNavigator.CurrentPageChanged += HandleCurrentPageChanged;
            App.SaveFileService.FilePathChanged += HandleFilePathChanged;
        }

        public async Task CheckForUpdateAsync()
        {
            var latest = await UpdateChecker.GetLatestVersionAsync();
            if (latest == null) return;

            if (UpdateChecker.IsNewerVersion(latest))
            {
                var latestFormatted = latest.Replace("rel-", string.Empty);

                bool result = ShowMessage.Info("Update available",
                    $"Version {latestFormatted} is now available. You are currently running {App.APP_VERSION}.\n\nDownload the latest update now?",
                    DialogOptions.YesNo,
                    App.Current.MainWindow,
                    DialogSound.Info,
                    DialogSize.Medium);

                if (result == true)
                {
                    Utils.OpenUrl("https://github.com/corv1njano/ScritchyScratchyCheater/releases/latest");
                }
            }
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
