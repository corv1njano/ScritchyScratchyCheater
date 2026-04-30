using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ScritchyScratchyCheater.Services;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Pages;
using System.IO;
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

            CreateBackup(filePath);
            
            switch (loadResult.Version)
            {
                case "0.1":
                    App.PageNavigator.Navigate(new EditorV01());
                    break;
                default:
                    break;
            }
        }

        private void CreateBackup(string sourcePath)
        {
            var result = ShowMessage.Neutral("Create backup?",
                "Do you want to create a backup of your save file before editing?",
                DialogOptions.YesNo);

            if (result == false) return;

            var folderPicker = new OpenFolderDialog
            {
                Title = "Select a directory to save the backup copy of your file...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (folderPicker.ShowDialog() == true)
            {
                var dir = folderPicker.FolderName;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string fileName = $"save_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json.backup";
                string destinationPath = Path.Combine(dir, fileName);

                File.Copy(sourcePath, destinationPath, true);

                ShowMessage.Info("Backup done",
                    "A backup of the save file has been created successfully.",
                    DialogOptions.Ok);
            }
        }
    }
}
