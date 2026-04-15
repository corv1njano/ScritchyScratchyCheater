using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Views.Pages
{
    /// <summary>
    /// Interaction logic for SaveEditorV01.xaml
    /// </summary>
    public partial class SaveEditorV01 : Page
    {
        private bool _isUpdatingUi = false; // use for checkbox

        public SaveEditorV01()
        {
            InitializeComponent();
            App.SaveFileService.SaveFileChanged += OnSaveFileChanged;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await App.ResourceParser.LoadSpritesAsync();

            if (App.SaveFileService.LoadedSaveFile is SaveFileV01 sf)
            {
                LoadContentsIntoUi(sf);
            }

            // tab progress
            MoneyIcon.Source = App.ResourceParser.GetSprite("money");
            PrestigeIcon.Source = App.ResourceParser.GetSprite("prestige");
            TokenIcon.Source = App.ResourceParser.GetSprite("token");
            SoulIcon.Source = App.ResourceParser.GetSprite("soul");
        }

        private void OnSaveFileChanged(ISaveFile? saveFile)
        {
            if (saveFile == null) return;
            LoadContentsIntoUi((SaveFileV01)saveFile);
        }

        private void LoadContentsIntoUi(SaveFileV01 saveFile)
        {
            _isUpdatingUi = true;

            InputMoney.Text = saveFile.LayerOne!.Money.ToString(CultureInfo.InvariantCulture);
            InputPrestige.Text = saveFile.PrestigeCurrency.ToString();
            InputTokens.Text = saveFile.Tokens.ToString();
            InputSouls.Text = saveFile.LayerOne.Souls.ToString();

            _isUpdatingUi = false;
        }

        #region tab progress
        private void MaxMoney_Click(object sender, RoutedEventArgs e)
        {
            InputMoney.Text = double.MaxValue.ToString(CultureInfo.InvariantCulture);
        }
        private void MaxPrestige_Click(object sender, RoutedEventArgs e)
        {
            InputPrestige.Text = int.MaxValue.ToString(CultureInfo.InvariantCulture);
        }
        private void MaxTokens_Click(object sender, RoutedEventArgs e)
        {
            InputTokens.Text = int.MaxValue.ToString(CultureInfo.InvariantCulture);
        }
        private void MaxSouls_Click(object sender, RoutedEventArgs e)
        {
            InputSouls.Text = int.MaxValue.ToString(CultureInfo.InvariantCulture);
        }

        private void InputMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(InputMoney.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                if (value < 0 || value > double.MaxValue)
                {
                    InputMoney.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
                }
                else
                {
                    InputMoney.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border");
                }
            }
            else
            {
                InputMoney.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
            }
        }
        private void InputPrestige_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(InputPrestige.Text, out int value))
            {
                if (value < 0 || value > int.MaxValue)
                {
                    InputPrestige.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
                }
                else
                {
                    InputPrestige.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border");
                }
            }
            else
            {
                InputPrestige.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
            }
        }
        private void InputTokens_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(InputTokens.Text, out int value))
            {
                if (value < 0 || value > int.MaxValue)
                {
                    InputTokens.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
                }
                else
                {
                    InputTokens.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border");
                }
            }
            else
            {
                InputTokens.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
            }
        }
        private void InputSouls_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(InputSouls.Text, out int value))
            {
                if (value < 0 || value > int.MaxValue)
                {
                    InputSouls.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
                }
                else
                {
                    InputSouls.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border");
                }
            }
            else
            {
                InputSouls.BorderBrush = (SolidColorBrush)Application.Current.TryFindResource("ControlElement.Flat.Border.Invalid");
            }
        }
        #endregion

        private async void SaveCurrentChanges_Click(object sender, RoutedEventArgs e)
        {
            await SaveChanges();
        }

        private async Task SaveChanges()
        {
            bool valid = UpdateSaveFilemFromUi();

            if (!valid)
            {
                ShowMessage.Error("Saving prohibited",
                    "Some input fields are invalid. The save file cannot be updated.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.Ok);
                return;
            }

            bool result = await App.SaveFileService.Save();

            if (result)
            {
                ShowMessage.Info("File saved",
                    "The save file was updated successfully.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.Ok);
            }
            else
            {
                ShowMessage.Error("Saving failed",
                    "Unable to update the save file.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.Ok);
            }
        }

        private bool UpdateSaveFilemFromUi()
        {
            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 save) return false;

            bool isValid = true;

            if (double.TryParse(InputMoney.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double money))
            {
                save.LayerOne!.Money = money;
            }
            else { isValid = false; }

            if (int.TryParse(InputPrestige.Text, out int prestige))
            {
                save.PrestigeCurrency = prestige;
            }
            else { isValid = false; }

            if (int.TryParse(InputTokens.Text, out int tokens))
            {
                save.Tokens = tokens;
            }
            else { isValid = false; }

            if (int.TryParse(InputSouls.Text, out int souls))
            {
                save.LayerOne!.Souls = souls;
            }
            else { isValid = false; }

            return isValid;
        }

        private async void ReloadSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = ShowMessage.Warning("Reloading Save File",
                "Are you sure you want to reload the save file? Any unsaved changes will be lost.",
                App.Current.MainWindow,
                Dialogs.MessageDialog.DialogOptions.YesNo);

            if (dialogResult == true)
            {
                var oldPath = App.SaveFileService.CurrentFilePath;
                var oldVersion = App.SaveFileService.CurrentSaveFileVersion;

                var (result, version) = await App.SaveFileService.Initialize(oldPath);

                if (!result || version == null)
                {
                    ShowMessage.Error("Reloading failed",
                        "Unable to reload the save file. The file will now be closed.",
                        App.Current.MainWindow,
                        Dialogs.MessageDialog.DialogOptions.Ok);

                    App.SaveFileService.Reset();
                    App.PageNavigator.Navigate(new StartPage());
                }
                else if (result ==  true && version == oldVersion)
                {
                    ShowMessage.Info("File reloaded",
                        "The save file was reloaded successfully.",
                        App.Current.MainWindow,
                        Dialogs.MessageDialog.DialogOptions.Ok);
                }
            }
        }

        private async void CloseSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (App.SaveFileService.LoadedSaveFile != null)
            {
                var result = ShowMessage.Warning("Save before Closing",
                    "Are you sure you want to close the save file? Any unsaved changes will be lost.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.YesNo);

                if (result == true)
                {
                    App.SaveFileService.Reset();
                    App.PageNavigator.Navigate(new StartPage());
                }
                else
                {
                    return;
                }
            }
        }

        private void OpenFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            string filePath = App.SaveFileService.CurrentFilePath;

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                ShowMessage.Error("File not found",
                    "Unable to locate the save file. It may have been moved, renamed, or deleted.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.Ok);
                return;
            }

            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }
    }
}
