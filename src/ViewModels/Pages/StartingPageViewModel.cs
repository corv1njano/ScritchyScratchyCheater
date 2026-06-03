using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ScritchyScratchyCheater.Utilities;

namespace ScritchyScratchyCheater.ViewModels.Pages
{
    public partial class StartingPageViewModel : ObservableObject
    {
        public StartingPageViewModel() { }

        [RelayCommand]
        private async Task LoadDefaultSaveFile()
        {
            string defaultPath = App.SaveFileService.DefaultSaveFilePath;
            await SaveFileHelper.LoadSaveFile(defaultPath, true);
        }

        [RelayCommand]
        private async Task BrowseSaveFile()
        {
            var filePicker = new OpenFileDialog
            {
                Title = "Select a Scritchy Scratchy Save File...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Save/Backup File (*.json, *.backup)|*.json;*.backup",
                Multiselect = false
            };

            if (filePicker.ShowDialog() == true)
            {
                await SaveFileHelper.LoadSaveFile(filePicker.FileName);
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
    }
}
