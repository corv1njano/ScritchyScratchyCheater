using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.Models.Results;
using ScritchyScratchyCheater.Models.SaveFiles;
using ScritchyScratchyCheater.Utilities;
using ScritchyScratchyCheater.ViewModels.Data;
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
        private bool _isSaving = false;
        public bool CanSave => !_isSaving && CheckCanSave();

        public EditorV01ViewModel()
        {
            App.SaveFileService.SaveFileChanged += HandleSaveFileChanged;

            LoadDataToUi();

            TicketsView = CollectionViewSource.GetDefaultView(Tickets);
            TicketsView.Filter = Utils.CreateNameFilter<TicketItem>(t => t.Ticket?.Name, () => SearchTicket);

            PrestigeUpgradesView = CollectionViewSource.GetDefaultView(PrestigeUpgrades);
            PrestigeUpgradesView.Filter = Utils.CreateNameFilter<PrestigeUpgradeItem>(p => p.PrestigeUpgrade?.Name, () => SearchPrestigeUpgrade);

            UpgradesView = CollectionViewSource.GetDefaultView(Upgrades);
            UpgradesView.Filter = Utils.CreateNameFilter<UpgradeItem>(u => u.Upgrade?.Name, () => SearchUpgrade);

            AchievementsView = CollectionViewSource.GetDefaultView(Achievements);
            AchievementsView.Filter = Utils.CreateNameFilter<AchievementItem>(a => a.Achievement?.Name, () => SearchAchievement);
        }

        private bool CheckCanSave()
        {
            return IsMoneyValid
                && IsPrestigeValid
                && IsTokensValid
                && IsSoulsValid
                && IsTicketLevelValid
                && IsMachineTierValid
                && IsEelectricFanChargeValid
                && IsEggTimerChargeValid
                && Upgrades.All(u => u.IsBuyCountValid)
                && PrestigeUpgrades.All(u => u.IsBuyCountValid);
        }

        /// <summary>
        /// Asynchronously loads and initializes the user interface (like icons etc.).
        /// </summary>
        /// <returns>A task that represents the async operation.</returns>
        public async Task LoadUiAsync()
        {
            await App.ResourceParser.LoadSpritesAsync();

            MoneyIcon = App.ResourceParser.GetSprite("mMoney");
            PrestigeIcon = App.ResourceParser.GetSprite("mPrestige");
            TokensIcon = App.ResourceParser.GetSprite("mToken");
            SoulsIcon = App.ResourceParser.GetSprite("mSoul");
            StarIcon = App.ResourceParser.GetSprite("mStar");
            CyanStarIcon = App.ResourceParser.GetSprite("mCyanStar");
            TheMachine = App.ResourceParser.GetSprite("uTheMachine");
            ElectricFanIcon = App.ResourceParser.GetSprite("uFan");
            EggTimerIcon = App.ResourceParser.GetSprite("uEggTimer");
            MundoIcon = App.ResourceParser.GetSprite("uMundo");
            TrashCanIcon = App.ResourceParser.GetSprite("uTrashCan");

            TicketsView.Refresh();
            PrestigeUpgradesView.Refresh();
            UpgradesView.Refresh();
            AchievementsView.Refresh();

            // lists, that are independent from loaded save file data, thus they need to bet set to index 0
            SelectedCatalog = Catalogs[0];
            SelectedCosmeticCategory = CosmeticCategories[0];

            UpdateCurrentCatalogImage();
            UpdateCurrentCosmeticImage();
        }

        #region loading data
        private void LoadDataToUi()
        {
            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 sf) return;

            LoadProgress(sf);
            LoadUpgrades(sf);
            LoadAchievements(sf);
            LoadCosmetics(sf);
        }
        
        private void LoadProgress(SaveFileV01 sf)
        {
            if (sf == null || sf.LayerOne == null) return;

            MoneyText = SaveFileHelper.SanitizeDouble(sf.LayerOne!.Money).ToString(CultureInfo.InvariantCulture);
            SoulsText = sf.LayerOne.Souls.ToString();
            MachineTierText = sf.LayerOne.MachineTier.ToString();
            SelectedAct = Acts.FirstOrDefault(a => a.ActId == sf.CurrentAct) ?? Acts.First(a => a.ActId == 1);

            var claiemdCatalogs = sf.LayerOne.ClaimedCustomTableItems ?? new List<string>();

            Catalogs.Clear();
            foreach (var catalog in App.GameDataParser.GetCatalogs())
            {
                var id = catalog.Id;
                Catalogs.Add(new CatalogItem
                {
                    Catalog = catalog,
                    IsClaimed = claiemdCatalogs.Contains(id)
                });
            }
            SelectedCatalog = Catalogs.Count > 0 ? Catalogs[0] : null;

            var ticketsGottenJackpot = sf.LayerOne.JackpotsGotten ?? new List<string>();
            var ticketsGottenSuperJackpot = sf.LayerOne.SuperJackpotsGotten ?? new List<string>();
            var ticketDic = sf.LayerOne.TicketProgressionDict ?? new Dictionary<string, TicketDataV01>();

            Tickets.Clear();
            foreach (var ticket in App.GameDataParser.GetTickets())
            {
                var id = ticket.Id;
                var progression = ticketDic.TryGetValue(id, out var progress) ? progress : null;
                var level = progression?.Level ?? 0;

                Tickets.Add(new TicketItem
                {
                    Ticket = ticket,
                    Xp = progression!.Xp,
                    Level = level is < 0 or > MAX_TICKET_LEVEL ? 0 : level,
                    GottenJackpot = ticketsGottenJackpot.Contains(id),
                    GottenSuperJackpot = ticketsGottenSuperJackpot.Contains(id)
                });
            }
        }
        
        private void LoadUpgrades(SaveFileV01 sf)
        {
            if (sf == null || sf.LayerOne == null) return;

            PrestigeText = sf.PrestigeCurrency.ToString();

            _deathByFinalChanceCount = sf.DeathByFinalChanceCount;
            var prestigeUpgradeDic = sf.BoughtPrestigeUpgrades ?? new Dictionary<string, int>();

            PrestigeUpgrades.Clear();
            foreach (var prestigeUpgrade in App.GameDataParser.GetPrestigeUpgrades())
            {
                var item = new PrestigeUpgradeItem
                {
                    PrestigeUpgrade = prestigeUpgrade,
                    BuyCountText = (prestigeUpgradeDic.TryGetValue(prestigeUpgrade.Id, out var value) ? value : 0).ToString()
                };

                // validate input and update final chance death counter if needed
                item.PropertyChanged += (_, e) =>
                {
                    OnPropertyChanged(nameof(CanSave));
                    if (e.PropertyName == nameof(PrestigeUpgradeItem.BuyCountText)) UpdateDeathCount(item);
                };
                PrestigeUpgrades.Add(item);
            }

            foreach (var item in PrestigeUpgrades) UpdateDeathCount(item);

            var upgradeDic = sf.LayerOne.UpgradeDataDict ?? new Dictionary<string, UpgradeDataV01>();

            Upgrades.Clear();
            foreach (var upgrade in App.GameDataParser.GetUpgrades())
            {
                var item = new UpgradeItem
                {
                    Upgrade = upgrade,
                    BuyCountText = (upgradeDic.TryGetValue(upgrade.Id, out var value) ? value.BuyCount : 0).ToString()
                };

                // subscribe to make GadgetItem ViewModel know when validity check changes
                item.PropertyChanged += (_, _) => OnPropertyChanged(nameof(CanSave));
                Upgrades.Add(item);
            }
        }
        
        private void LoadAchievements(SaveFileV01 sf)
        {
            if (sf == null) return;

            var achievementsGotten = sf.AchievementsGotten ?? new List<string>();
            var achievementsClaimed = sf.AchievementsClaimed ?? new List<string>();

            Achievements.Clear();
            foreach (var achievement in App.GameDataParser.GetAchievements())
            {
                var id = achievement.Id;
                Achievements.Add(new AchievementItem
                {
                    Achievement = achievement,
                    IsUnlocked = achievementsGotten.Contains(id),
                    IsClaimed = achievementsClaimed.Contains(id),
                });
            }
        }
        
        private void LoadCosmetics(SaveFileV01 sf)
        {
            if (sf == null || sf.LayerOne == null) return;

            TokensText = sf.Tokens.ToString();

            var cosmeticsPurchased = sf.BoughtCosmetics ?? new List<string>();
            var cosmeticsEquipped = sf.EquippedCosmetics ?? new List<string>();

            Cosmetics.Clear();
            foreach (var cosmetic in App.GameDataParser.GetCosmetics())
            {
                var id = cosmetic.Id;
                Cosmetics.Add(new CosmeticItem
                {
                    Cosmetic = cosmetic,
                    IsPurchased = cosmeticsPurchased.Contains(id),
                    IsEquipped = cosmeticsEquipped.Contains(id),
                });
            }

            ElectricFanChargeText = SaveFileHelper.SanitizeDouble(sf.LayerOne.ElectricFanChargeLeft).ToString(CultureInfo.InvariantCulture);
            EggTimerChargeText = SaveFileHelper.SanitizeDouble(sf.LayerOne.EggTimerChargeLeft).ToString(CultureInfo.InvariantCulture);
            IsElectricFanPaused = sf.LayerOne.FanPaused;
            IsMundoDead = sf.LayerOne.MundoDead;
            IsTrashCanDead = sf.LayerOne.TrashCanDead;
        }
        #endregion

        #region saveing data
        [RelayCommand]
        private async Task SaveChanges()
        {
            if (!CanSave) return;
            _isSaving = true;

            if (App.SaveFileService.LoadedSaveFile is not SaveFileV01 sf) return;

            SaveProgress(sf);
            SaveUpgrades(sf);
            SaveAchievements(sf);
            SaveCosmetics(sf);

            SaveResult result = await App.SaveFileService.SaveAsync();
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

            _isSaving = false;
        }

        private void SaveProgress(SaveFileV01 sf)
        {
            if (sf == null || sf.LayerOne == null) return;

            var catalogsClaimed = new HashSet<string>();

            foreach (var catalog in Catalogs)
            {
                var id = catalog.Catalog?.Id;
                if (catalog.Catalog == null || string.IsNullOrWhiteSpace(id)) continue;

                if (catalog.IsClaimed) catalogsClaimed.Add(id);
            }

            var ticketsGottenJackpot = new HashSet<string>();
            var ticketsGottenSuperJackpot = new HashSet<string>();
            var layer = sf.LayerOne;

            foreach (var ticket in Tickets)
            {
                var id = ticket.Ticket?.Id;
                if (ticket.Ticket == null || string.IsNullOrWhiteSpace(id)) continue;

                if (ticket.GottenJackpot) ticketsGottenJackpot.Add(id);
                if (ticket.GottenSuperJackpot) ticketsGottenSuperJackpot.Add(id);

                if (layer.TicketProgressionDict!.ContainsKey(id))
                {
                    layer.TicketProgressionDict[id] = new TicketDataV01
                    {
                        Id = id,
                        Xp = ticket.Xp,
                        Level = ticket.Level
                    };
                }
            }

            layer.Money = double.Parse(MoneyText);
            layer.Souls = int.Parse(SoulsText);
            layer.MachineTier = int.Parse(MachineTierText);
            sf.CurrentAct = SelectedAct == null ? 1 : SelectedAct.ActId;
            layer.ClaimedCustomTableItems = catalogsClaimed.ToList();
            layer.JackpotsGotten = ticketsGottenJackpot.ToList();
            layer.SuperJackpotsGotten = ticketsGottenSuperJackpot.ToList();
        }

        private void SaveUpgrades(SaveFileV01 sf)
        {
            if (sf == null || sf.LayerOne == null) return;

            foreach (var entry in PrestigeUpgrades)
            {
                var id = entry.PrestigeUpgrade?.Id;
                if (entry.PrestigeUpgrade == null || string.IsNullOrWhiteSpace(id)) continue;

                if (sf.BoughtPrestigeUpgrades!.ContainsKey(id))
                {
                    sf.BoughtPrestigeUpgrades[id] = int.TryParse(entry.BuyCountText, out var count) ? count : 0;
                }
            }

            foreach (var upgrade in Upgrades)
            {
                var id = upgrade.Upgrade?.Id;
                if (upgrade.Upgrade == null || string.IsNullOrWhiteSpace(id)) continue;

                if (sf.LayerOne!.UpgradeDataDict!.ContainsKey(id))
                {
                    sf.LayerOne.UpgradeDataDict[id] = new UpgradeDataV01
                    {
                        Id = id,
                        BuyCount = int.Parse(upgrade.BuyCountText)
                    };
                }
            }

            sf.PrestigeCurrency = int.Parse(PrestigeText);
            sf.DeathByFinalChanceCount = _deathByFinalChanceCount;
        }

        private void SaveAchievements(SaveFileV01 sf)
        {
            if (sf == null) return;

            var gottenAchievements = new HashSet<string>();
            var claimedAchievements = new HashSet<string>();

            foreach (var achievement in Achievements)
            {
                var id = achievement.Achievement?.Id;
                if (achievement.Achievement == null || string.IsNullOrWhiteSpace(id)) continue;

                if (achievement.IsUnlocked) gottenAchievements.Add(id);
                if (achievement.IsClaimed) claimedAchievements.Add(id);
            }

            sf.AchievementsGotten = gottenAchievements.ToList();
            sf.AchievementsClaimed = claimedAchievements.ToList();
        }

        private void SaveCosmetics(SaveFileV01 sf)
        {
            if (sf == null) return;

            var purchasedCosmetics = new HashSet<string>();
            var equippedCosmetics = new HashSet<string>();

            foreach (var cosmetic in Cosmetics)
            {
                var id = cosmetic.Cosmetic?.Id;
                if (cosmetic.Cosmetic == null || string.IsNullOrWhiteSpace(id)) continue;

                if (cosmetic.IsPurchased) purchasedCosmetics.Add(id);
                if (cosmetic.IsEquipped) equippedCosmetics.Add(id);
            }

            sf.Tokens = int.Parse(TokensText);
            sf.BoughtCosmetics = purchasedCosmetics.ToList();
            sf.EquippedCosmetics = equippedCosmetics.ToList();

            sf.LayerOne!.ElectricFanChargeLeft = double.Parse(ElectricFanChargeText);
            sf.LayerOne.EggTimerChargeLeft = double.Parse(EggTimerChargeText);
            sf.LayerOne.FanPaused = IsElectricFanPaused;
            sf.LayerOne.MundoDead = IsMundoDead;
            sf.LayerOne.TrashCanDead = IsTrashCanDead;
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

            LoadResult loadResult = await App.SaveFileService.LoadAsync(oldPath);

            if (loadResult.Version != oldVersion)
            {
                ShowMessage.Error("Reloading failed",
                    $"The loaded save file version '{loadResult.Version}' does not match the previous version '{oldVersion}'. Return to the start page.",
                    DialogOptions.Ok);

                App.SaveFileService.Reset();
                App.PageNavigator.Navigate(new StartingPage());
                return;
            }

            if (!loadResult.Success)
            {
                ShowMessage.Error("Reloading failed",
                    loadResult.StatusMessage,
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
        private void CreateBackup()
        {
            SaveFileHelper.CreateBackup();
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
}
