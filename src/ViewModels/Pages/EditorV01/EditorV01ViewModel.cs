using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.Models.GameData;
using ScritchyScratchyCheater.Models.Results;
using ScritchyScratchyCheater.Models.SaveFiles;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.Views.Pages;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.ViewModels.Pages.EditorV01
{
    internal partial class EditorV01ViewModel : ObservableObject
    {
        public bool CanSave => IsMoneyValid && IsPrestigeValid && IsTokensValid && IsSoulsValid;

        public EditorV01ViewModel()
        {
            App.SaveFileService.SaveFileChanged += HandleSaveFileChanged;

            AchievementsView = CollectionViewSource.GetDefaultView(Achievements);
            AchievementsView.Filter = item =>
                item is AchievementItem entry
                && (string.IsNullOrWhiteSpace(SearchAchievement)
                || entry.Achievement!.Name.Contains(SearchAchievement, StringComparison.OrdinalIgnoreCase));

            LoadDataToUi();

            SelectedCosmeticCategory = CosmeticCategories[0];
            SelectedCosmetic = FilteredCosmetics.FirstOrDefault();
            UpdateCurrentCosmeticImage();
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

        private void LoadDataToUi()
        {
            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 sf) return;

            MoneyText = SaveFileHelper.SanitizeDouble(sf.LayerOne!.Money).ToString(CultureInfo.InvariantCulture);
            PrestigeText = sf.PrestigeCurrency.ToString();
            TokensText = sf.Tokens.ToString();
            SoulsText = sf.LayerOne.Souls.ToString();

            var achievementsGotten = sf.AchievementsGotten ?? new List<string>();
            var achievementsClaimed = sf.AchievementsClaimed ?? new List<string>();

            Achievements.Clear();
            foreach (var achievement in App.GameDataParser.GetAchievements())
            {
                Achievements.Add(new AchievementItem
                {
                    Achievement = achievement,
                    IsUnlocked = achievementsGotten.Contains(achievement.Id),
                    IsClaimed = achievementsClaimed.Contains(achievement.Id),
                });
            }

            var cosmeticsPurchased = sf.BoughtCosmetics ?? new List<string>();
            var cosmeticsEquipped = sf.EquippedCosmetics ?? new List<string>();

            Cosmetics.Clear();
            foreach (var cosmetic in App.GameDataParser.GetCosmetics())
            {
                Cosmetics.Add(new CosmeticItem
                {
                    Cosmetic = cosmetic,
                    IsPurchased = cosmeticsPurchased.Contains(cosmetic.Id),
                    IsEquipped = cosmeticsEquipped.Contains(cosmetic.Id),
                });
            }
        }

        [RelayCommand]
        private async Task SaveChanges()
        {
            if (!CanSave) return;
            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 sf) return;

            // currencies
            sf.LayerOne!.Money = double.Parse(MoneyText);
            sf.PrestigeCurrency = int.Parse(PrestigeText);
            sf.LayerOne!.Souls = int.Parse(SoulsText);

            SaveAchievements(sf);
            SaveCosmetics(sf);

            SaveResult result = await App.SaveFileService.Save();
            if (result.Success)
            {
                ShowMessage.Info("File saved",
                    result.StatusMessage,
                    DialogOptions.Ok);
            }
            else if (!result.Success)
            {
                ShowMessage.Error("Saving failed",
                    result.StatusMessage,
                    DialogOptions.Ok);
            }
        }

        #region save methods
        private void SaveAchievements(SaveFileV01 sf)
        {
            var gottenAchievements = new HashSet<string>();
            var claimedAchievements = new HashSet<string>();

            foreach (var entry in Achievements)
            {
                var id = entry.Achievement?.Id;
                if (string.IsNullOrWhiteSpace(id)) continue;

                if (entry.IsUnlocked) gottenAchievements.Add(id);
                if (entry.IsClaimed) claimedAchievements.Add(id);
            }

            sf.AchievementsGotten = gottenAchievements.ToList();
            sf.AchievementsClaimed = claimedAchievements.ToList();
        }

        private void SaveCosmetics(SaveFileV01 sf)
        {
            var purchasedCosmetics = new HashSet<string>();
            var equippedCosmetics = new HashSet<string>();

            foreach (var entry in Cosmetics)
            {
                var id = entry.Cosmetic?.Id;
                if (entry.Cosmetic == null || string.IsNullOrWhiteSpace(id)) continue;

                if (entry.IsPurchased) purchasedCosmetics.Add(id);
                if (entry.IsEquipped) equippedCosmetics.Add(id);
            }

            sf.Tokens = int.Parse(TokensText);
            sf.BoughtCosmetics = purchasedCosmetics.ToList();
            sf.EquippedCosmetics = equippedCosmetics.ToList();
        }
        #endregion

        [RelayCommand]
        private async Task ReloadSaveFile()
        {
            var dialogResult = ShowMessage.Warning("Reloading File",
                "Are you sure you want to reload the save file? Any unsaved changes will be lost.",
                DialogOptions.YesNo);

            if (dialogResult == false) return;

            var oldPath = App.SaveFileService.CurrentFilePath;
            var oldVersion = App.SaveFileService.CurrentSaveFileVersion;

            if (string.IsNullOrWhiteSpace(oldPath))
            {
                ShowMessage.Error("Reloading failed",
                    "The file location could not be determined.",
                    DialogOptions.Ok);
                return;
            }

            LoadResult loadResult = await App.SaveFileService.Load(oldPath);

            if (!loadResult.Success)
            {
                ShowMessage.Error("Reloading failed",
                    loadResult.StatusMessage,
                    DialogOptions.Ok);

                App.SaveFileService.Reset();
                App.PageNavigator.Navigate(new StartingPage());
                return;
            }

            if (loadResult.Version != oldVersion)
            {
                ShowMessage.Error("Reloading failed",
                    $"The loaded save file version '{loadResult.Version}' does not match the previous version '{oldVersion}'. Return to the start page.",
                    DialogOptions.Ok);

                App.SaveFileService.Reset();
                App.PageNavigator.Navigate(new StartingPage());
                return;
            }

            ShowMessage.Info("File reloaded",
                loadResult.StatusMessage,
                DialogOptions.Ok);
        }

        [RelayCommand]
        private void CloseSaveFile()
        {
            if (App.SaveFileService.LoadedSaveFile != null)
            {
                var result = ShowMessage.Warning("Closing File",
                    "Are you sure you want to close the save file? Any unsaved changes will be lost.",
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
                    DialogOptions.Ok);
                return;
            }

            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }

        private void HandleSaveFileChanged(ISaveFile? saveFile)
        {
            if (saveFile == null) return;
            LoadDataToUi();
        }
    }

    #region data containers
    public sealed partial class AchievementItem : ObservableObject
    {
        public Achievement? Achievement { get; init; }

        [ObservableProperty]
        private bool _isUnlocked;

        [ObservableProperty]
        private bool _isClaimed;
    }

    public sealed partial class CosmeticItem : ObservableObject
    {
        public Cosmetic? Cosmetic { get; init; }

        [ObservableProperty]
        private bool _isPurchased;

        [ObservableProperty]
        private bool _isEquipped;

        public string DisplayName => Cosmetic?.Name ?? "None (default)";
    }
    #endregion
}
