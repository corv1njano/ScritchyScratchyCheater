using ScritchyScratchyCheater.Models.Results;
using ScritchyScratchyCheater.Views.Dialogs;
using ScritchyScratchyCheater.Views.Pages;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.Utilities
{
    /// <summary>
    /// Provides helper methods for working with the save file.
    /// </summary>
    public static class SaveFileHelper
    {
        /// <summary>
        /// Returns a sanitized double value by replacing NaN or infinity with a large finite number.
        /// </summary>
        /// <param name="value">The double value to sanitize. If the value is NaN or infinity, it will be replaced with a large finite number.</param>
        /// <returns>The original value if it is a finite number; otherwise, a large finite number.</returns>
        public static double SanitizeDouble(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) return 1e300;

            return value;
        }

        /// <summary>
        /// Loads a save file from the specified file path and navigates to the appropriate editor page.
        /// </summary>
        /// <param name="filePath">The full path to the save file to load. If null or empty, the method returns immediately.</param>
        /// <param name="isLoadedFromDefaultPath">Indicates whether the file is being loaded from the default save file path. Defaults to false.</param>
        /// <returns>A task representing the asynchronous load operation.</returns>
        public static async Task LoadSaveFile(string? filePath, bool isLoadedFromDefaultPath = false)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            LoadResult loadResult = await App.SaveFileService.LoadAsync(filePath, isLoadedFromDefaultPath);

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

        /// <summary>
        /// Opens the backup creation dialog, allowing the user to create a backup of the currently loaded save file.
        /// </summary>
        public static void CreateBackup()
        {
            var dialog = new CreateBackupDialog()
            {
                Owner = App.Current.MainWindow
            };
            dialog.ShowDialog();
        }
    }
}
