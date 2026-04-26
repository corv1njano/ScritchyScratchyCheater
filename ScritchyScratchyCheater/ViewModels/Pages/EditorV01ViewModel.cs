using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using ScritchyScratchyCheater.Services;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.ViewModels.Pages
{
    internal partial class EditorV01ViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsMoneyValid))]
        private string _moneyText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPrestigeValid))]
        private string _prestigeText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTokensValid))]
        private string _tokensText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSoulsValid))]
        private string _soulsText = string.Empty;

        public bool IsMoneyValid => double.TryParse(MoneyText, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        public bool IsPrestigeValid => int.TryParse(PrestigeText, out _);
        public bool IsTokensValid => int.TryParse(TokensText, out _);
        public bool IsSoulsValid => int.TryParse(SoulsText, out _);

        [ObservableProperty]
        private string _searchAchievement = string.Empty;
        public ObservableCollection<AchievementEntry> Achievements { get; } = new();
        public ICollectionView AchievementsView { get; }

        [ObservableProperty]
        private ImageSource? _moneyIcon;
        [ObservableProperty]
        private ImageSource? _prestigeIcon;
        [ObservableProperty]
        private ImageSource? _tokensIcon;
        [ObservableProperty]
        private ImageSource? _soulsIcon;

        public EditorV01ViewModel()
        {
            App.SaveFileService.SaveFileChanged += HandleSaveFileChanged;

            AchievementsView = CollectionViewSource.GetDefaultView(Achievements);
            AchievementsView.Filter = item =>
                item is AchievementEntry entry
                && (string.IsNullOrWhiteSpace(SearchAchievement)
                || entry.Achievement!.Name.Contains(SearchAchievement, StringComparison.OrdinalIgnoreCase));

            LoadSaveFileDataToUi();
        }

        /// <summary>
        /// Asynchronously loads and initializes the user interface (like icons etc.).
        /// </summary>
        /// <returns>A task that represents the async operation.</returns>
        public async Task LoadUiAsync()
        {
            await App.ResourceParser.LoadSpritesAsync();

            MoneyIcon = App.ResourceParser.GetSprite("money");
            PrestigeIcon = App.ResourceParser.GetSprite("prestige");
            TokensIcon = App.ResourceParser.GetSprite("token");
            SoulsIcon = App.ResourceParser.GetSprite("soul");
        }

        private void LoadSaveFileDataToUi()
        {
            if (App.SaveFileService.LoadedSaveFile is SaveFileV01 sf)
            {
                MoneyText = sf.LayerOne!.Money.ToString(CultureInfo.InvariantCulture);
                PrestigeText = sf.PrestigeCurrency.ToString();
                TokensText = sf.Tokens.ToString();
                SoulsText = sf.LayerOne.Souls.ToString();

                var gotten = sf.AchievementsGotten ?? new List<string>();
                var claimed = sf.AchievementsClaimed ?? new List<string>();

                Achievements.Clear();
                foreach (var achievement in App.GameDataParser.GetAchievements())
                {
                    Achievements.Add(new AchievementEntry
                    {
                        Achievement = achievement,
                        IsUnlocked = gotten.Contains(achievement.Id),
                        IsClaimed = claimed.Contains(achievement.Id),
                    });
                }
            }
        }

        [RelayCommand]
        private void MaxMoney()
        {
            MoneyText = double.MaxValue.ToString(CultureInfo.InvariantCulture);
        }

        [RelayCommand]
        private void MaxPrestige()
        {
            PrestigeText = int.MaxValue.ToString();
        }

        [RelayCommand]
        private void MaxTokens()
        {
            TokensText = int.MaxValue.ToString();
        }

        [RelayCommand]
        private void MaxSouls()
        {
            SoulsText = int.MaxValue.ToString();
        }

        [RelayCommand]
        private void UnlockAllAchievements()
        {
            foreach (var entry in Achievements)
            {
                entry.IsUnlocked = true;
            }
        }

        [RelayCommand]
        private void ClaimAllAchievements()
        {
            foreach (var entry in Achievements)
            {
                entry.IsClaimed = true;
            }
        }

        [RelayCommand]
        private async Task SaveChanges()
        {
            if (!IsMoneyValid || !IsPrestigeValid || !IsTokensValid || !IsSoulsValid)
            {
                ShowMessage.Error("Saving prohibited",
                    "Some input fields are invalid. The save file cannot be updated.",
                    App.Current.MainWindow,
                    DialogOptions.Ok);
                return;
            }

            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 sf) return;

            // currencies
            sf.LayerOne!.Money = double.Parse(MoneyText);
            sf.PrestigeCurrency = int.Parse(PrestigeText);
            sf.Tokens = int.Parse(TokensText);
            sf.LayerOne!.Souls = int.Parse(SoulsText);

            // achievements
            var gottenSet = new HashSet<string>();
            var claimedSet = new HashSet<string>();

            foreach (var entry in Achievements)
            {
                var id = entry.Achievement?.Id;
                if (string.IsNullOrWhiteSpace(id)) continue;

                if (entry.IsUnlocked) gottenSet.Add(id);
                if (entry.IsClaimed) claimedSet.Add(id);
            }

            sf.AchievementsGotten = gottenSet.ToList();
            sf.AchievementsClaimed = claimedSet.ToList();

            bool result = await App.SaveFileService.Save();
            if (result)
            {
                ShowMessage.Info("File saved",
                    "The save file was updated successfully.",
                    App.Current.MainWindow,
                    DialogOptions.Ok);
            }
            else
            {
                ShowMessage.Error("Saving failed",
                    "Unable to update the save file.",
                    App.Current.MainWindow,
                    DialogOptions.Ok);
            }
        }

        [RelayCommand]
        private async Task ReloadSaveFile()
        {
            var dialogResult = ShowMessage.Warning("Reloading File",
                "Are you sure you want to reload the save file? Any unsaved changes will be lost.",
                App.Current.MainWindow,
                DialogOptions.YesNo);

            if (dialogResult == true)
            {
                var oldPath = App.SaveFileService.CurrentFilePath;
                var oldVersion = App.SaveFileService.CurrentSaveFileVersion;

                // TODO: bool  as return type? is version needed here (or in general?)?
                var (result, version) = await App.SaveFileService.Initialize(oldPath);

                if (!result || version == null)
                {
                    ShowMessage.Error("Reloading failed",
                        "Unable to reload the save file. The file will now be closed.",
                        App.Current.MainWindow,
                        DialogOptions.Ok);

                    App.SaveFileService.Reset();
                    App.PageNavigator.Navigate(new StartingPage());
                }
                else if (result == true && version == oldVersion)
                {
                    ShowMessage.Info("File reloaded",
                        "The save file was reloaded successfully.",
                        App.Current.MainWindow,
                        DialogOptions.Ok);
                }
            }
        }

        [RelayCommand]
        private void CloseSaveFile()
        {
            if (App.SaveFileService.LoadedSaveFile != null)
            {
                var result = ShowMessage.Warning("Closing File",
                    "Are you sure you want to close the save file? Any unsaved changes will be lost.",
                    App.Current.MainWindow,
                    DialogOptions.YesNo);

                if (result == true)
                {
                    App.SaveFileService.Reset();
                    App.PageNavigator.Navigate(new StartingPage());
                }
            }
        }

        [RelayCommand]
        private void OpenFileExplorer()
        {
            string filePath = App.SaveFileService.CurrentFilePath;

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                ShowMessage.Error("File not found",
                    "Unable to locate the save file. It may have been moved, renamed, or deleted.",
                    App.Current.MainWindow,
                    DialogOptions.Ok);
                return;
            }

            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        partial void OnSearchAchievementChanged(string value)
        {
            AchievementsView.Refresh();
        }

        private void HandleSaveFileChanged(ISaveFile? saveFile)
        {
            if (saveFile == null) return;
            LoadSaveFileDataToUi();
        }
    }

    #region data containers
    public sealed partial class AchievementEntry : ObservableObject
    {
        public Achievement? Achievement { get; set; }

        [ObservableProperty]
        private bool _isUnlocked;

        [ObservableProperty]
        private bool _isClaimed;
    }
    #endregion
}
