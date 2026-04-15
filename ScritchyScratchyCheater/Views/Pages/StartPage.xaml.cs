using Microsoft.Win32;
using ScritchyScratchyCheater.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Views.Pages
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private async void LoadDefaultSaveFile_Click(object sender, RoutedEventArgs e)
        {
            string defaultPath = App.SaveFileService.DefaultSaveFilePath;
            await LoadSaveFile(defaultPath);
        }

        private async void BrowseSaveFile_Click(object sender, RoutedEventArgs e)
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
            var (result, version) = await App.SaveFileService.Initialize(filePath);

            if (!result) //  || version == null
            {
                ShowMessage.Error("Save File inavlid",
                    "The selected save file cannot be opened.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.Ok);
                return;
            }

            switch (version)
            {
                case "0.1":
                    App.PageNavigator.Navigate(new SaveEditorV01());
                    break;
                default:
                    ShowMessage.Error("Inavlid Version",
                        "The save version is not supported. Cannot open the save file.",
                        App.Current.MainWindow,
                        Dialogs.MessageDialog.DialogOptions.Ok);
                    break;
            }
        }
    }
}
