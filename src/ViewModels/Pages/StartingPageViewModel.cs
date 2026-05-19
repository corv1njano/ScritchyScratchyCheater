using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ScritchyScratchyCheater.Models.Results;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Dialogs;
using ScritchyScratchyCheater.Views.Pages;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.ViewModels.Pages
{
    internal partial class StartingPageViewModel : ObservableObject
    {
        public StartingPageViewModel() { }

        [RelayCommand]
        private async Task LoadDefaultSaveFile()
        {
            string defaultPath = App.SaveFileService.DefaultSaveFilePath;
            await LoadSaveFile(defaultPath);
        }

        [RelayCommand]
        private async Task BrowseSaveFile()
        {
            var filePicker = new OpenFileDialog
            {
                Title = "Select a Scritchy Scratchy Save File...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Save File (*.json)|*.json",
                Multiselect = false
            };

            if (filePicker.ShowDialog() == true)
            {
                await LoadSaveFile(filePicker.FileName);
            }
        }

        [RelayCommand]
        private void OpenGitHub()
        {
            Utils.OpenUrl("https://github.com/corv1njano/ScritchyScratchyCheater");
        }
        [RelayCommand]
        private void OpenSponsor()
        {
            Utils.OpenUrl("https://github.com/sponsors/corv1njano");
        }
        [RelayCommand]
        private void OpenSamples()
        {
            Utils.OpenUrl("https://github.com/corv1njano/ScritchyScratchyCheater/tree/master/samples");
        }

        private async Task LoadSaveFile(string filePath)
        {
            LoadResult loadResult =  await App.SaveFileService.Load(filePath);

            if (!loadResult.Success)
            {
                ShowMessage.Error("Loading failed",
                    loadResult.StatusMessage,
                    DialogOptions.Ok);
                return;
            }
            
            switch (loadResult.Version)
            {
                case "0.1":
                    App.PageNavigator.Navigate(new EditorV01Page());
                    break;
                default:
                    break;
            }

            CreateBackup();
        }

        private void CreateBackup()
        {
            var dialog = new CreateBackupDialog()
            {
                Owner = App.Current.MainWindow
            };
            dialog.ShowDialog();
        }
    }
}
