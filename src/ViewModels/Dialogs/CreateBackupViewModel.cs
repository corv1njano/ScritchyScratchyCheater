using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Dialogs;
using System.Diagnostics;
using System.IO;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.ViewModels.Dialogs
{
    internal partial class CreateBackupViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTargetPathValid))]
        [NotifyPropertyChangedFor(nameof(CanExport))]
        private string _targetPath = string.Empty;

        public bool IsTargetPathValid => !string.IsNullOrWhiteSpace(TargetPath)
            && Directory.Exists(TargetPath);

        public bool CanExport => IsTargetPathValid;

        [ObservableProperty]
        private bool _includeTimetamp = true;
        [ObservableProperty]
        private bool _openFolder;

        public CreateBackupViewModel()
        {
            TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        [RelayCommand]
        private void OpenTargetSelection()
        {
            var folderPicker = new OpenFolderDialog
            {
                Title = "Select a Directory to save the Backup...",
                InitialDirectory = Directory.Exists(TargetPath)
                    ? TargetPath
                    : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (folderPicker.ShowDialog() == true)
            {
                var dir = folderPicker.FolderName;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                TargetPath = dir;
            }
        }

        [RelayCommand]
        private void CreateBackup()
        {
            string sourcePath = App.SaveFileService.CurrentFilePath;

            string fileName = IncludeTimetamp
                ? $"save_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json.backup"
                : "save.json.backup";

            string destinationPath = Path.Combine(TargetPath, fileName);

            try
            {
                File.Copy(sourcePath, destinationPath, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error when creating backup: {ex}");

                //error message
                return;
            }

            if (OpenFolder)
            {
                if (string.IsNullOrWhiteSpace(destinationPath) || !File.Exists(destinationPath))
                {
                    ShowMessage.Error("File not found",
                        "Unable to locate the save file. It may have been moved, renamed, or deleted.",
                        DialogOptions.Ok);
                    return;
                }
                Process.Start("explorer.exe", $"/select,\"{destinationPath}\"");
            }

            App.Current.Windows.OfType<CreateBackupDialog>().FirstOrDefault()?.Close();

            //success message
        }
    }
}
