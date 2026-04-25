using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using ScritchyScratchyCheater.Services;
using ScritchyScratchyCheater.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditorV01.xaml
    /// </summary>
    public partial class EditorV01 : UserControl
    {
        private bool _isUpdatingUi = false; // use for checkbox

        private ObservableCollection<AchievementEntry> _achievements = new();
        private ICollectionView? _achievementsView;

        private string _searchAchievement = string.Empty;

        public EditorV01()
        {
            InitializeComponent();

            App.SaveFileService.SaveFileChanged += OnSaveFileChanged;
        }

        private async void Editor_Loaded(object sender, RoutedEventArgs e)
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

            var achievementsGotten = saveFile.AchievementsGotten ?? new List<string>();
            var achievementsClaimed = saveFile.AchievementsClaimed ?? new List<string>();
            _achievements.Clear();

            foreach (var achievement in App.GameDataParser.GetAchievements())
            {
                var id = achievement.Id;
                _achievements.Add(new AchievementEntry
                {
                    Achievement = achievement,
                    IsUnlocked = achievementsGotten.Contains(id),
                    IsClaimed = achievementsClaimed.Contains(id),
                });
            }

            _achievementsView = CollectionViewSource.GetDefaultView(_achievements);
            _achievementsView.Filter = FilterItems;

            AchievementsList.ItemsSource = _achievementsView;

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
                if (value < double.MinValue || value > double.MaxValue)
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
                if (value < int.MinValue || value > int.MaxValue)
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
                if (value < int.MinValue || value > int.MaxValue)
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
                if (value < int.MinValue || value > int.MaxValue)
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

        #region tab achievement
        private void UnlockAllAchievements_Click(object sender, RoutedEventArgs e)
        {
            foreach (var entry in _achievements)
            {
                entry.IsUnlocked = true;
            }
        }

        private void ClaimAllAchievements_Click(object sender, RoutedEventArgs e)
        {
            foreach (var entry in _achievements)
            {
                entry.IsClaimed = true; 
            }
        }

        private bool FilterItems(object obj)
        {
            if (obj is AchievementEntry achievement)
            {
                return string.IsNullOrWhiteSpace(_searchAchievement) || achievement.Achievement!.Name.Contains(_searchAchievement, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
        #endregion

        private async void SaveCurrentChanges_Click(object sender, RoutedEventArgs e)
        {
            await SaveChanges();
        }

        private async Task SaveChanges()
        {

            // input fields that require separate validation
            bool valid = ValidateInputFields();

            if (!valid)
            {
                ShowMessage.Error("Saving prohibited",
                    "Some input fields are invalid. The save file cannot be updated.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.Ok);
                return;
            }


            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 save) return;
            var gottenSet = new HashSet<string>();
            var claimedSet = new HashSet<string>();

            foreach (var entry in _achievements)
            {
                var id = entry.Achievement?.Id;
                if (string.IsNullOrWhiteSpace(id)) continue;

                if (entry.IsUnlocked) gottenSet.Add(id);
                if (entry.IsClaimed) claimedSet.Add(id);
            }

            save.AchievementsGotten = gottenSet.ToList();
            save.AchievementsClaimed = claimedSet.ToList();

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

        private bool ValidateInputFields()
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
            var dialogResult = ShowMessage.Warning("Reloading File",
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
                    App.PageNavigator.Navigate(new StartingPage());
                }
                else if (result == true && version == oldVersion)
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
                var result = ShowMessage.Warning("Closing File",
                    "Are you sure you want to close the save file? Any unsaved changes will be lost.",
                    App.Current.MainWindow,
                    Dialogs.MessageDialog.DialogOptions.YesNo);

                if (result == true)
                {
                    App.SaveFileService.Reset();
                    App.PageNavigator.Navigate(new StartingPage()); // cache it u dumb bitch!!!!!
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

        private void SearchAchievement_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                _searchAchievement = tb.Text;
                _achievementsView?.Refresh();
            }
        }
    }

    #region data containers
    public sealed class AchievementEntry : INotifyPropertyChanged
    {
        public Achievement? Achievement { get; set; }
        private bool _isUnlocked;
        private bool _isClaimed;

        public bool IsUnlocked
        {
            get => _isUnlocked;
            set
            {
                if (_isUnlocked != value)
                {
                    _isUnlocked = value;
                    OnPropertyChanged(nameof(IsUnlocked));
                }
            }
        }

        public bool IsClaimed
        {
            get => _isClaimed;
            set
            {
                if (_isClaimed != value)
                {
                    _isClaimed = value;
                    OnPropertyChanged(nameof(IsClaimed));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    #endregion
}

