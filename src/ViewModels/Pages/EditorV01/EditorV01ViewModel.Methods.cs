using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Models.GameData;
using ScritchyScratchyCheater.ViewModels.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ScritchyScratchyCheater.ViewModels.Pages.EditorV01
{
    internal partial class EditorV01ViewModel : ObservableObject
    {
        #region tab progress
        [RelayCommand]
        private void MaxMoney()
        {
            MoneyText = 1e300.ToString(CultureInfo.InvariantCulture);
        }

        [RelayCommand]
        private void MaxSouls()
        {
            SoulsText = int.MaxValue.ToString();
        }

        [RelayCommand]
        private void MaxMachineTier()
        {
            MachineTierText = MAX_MACHINE_TIER.ToString();
        }

        [RelayCommand]
        private void JackpotAllTickets()
        {
            foreach (var entry in Tickets) entry.GottenJackpot = true;
        }

        [RelayCommand]
        private void SuperJackpotAllTickets()
        {
            foreach (var entry in Tickets) entry.GottenSuperJackpot = true;
        }

        [RelayCommand]
        private void MaxTicketLevel()
        {
            if (SelectedTicket == null) return;

            SelectedTicket.Level = MAX_TICKET_LEVEL;
            if (SelectedTicket != null) TicketLevelText = MAX_TICKET_LEVEL.ToString();
        }

        [RelayCommand]
        private void MaxTicketLevelAll()
        {
            foreach (var entry in Tickets) entry.Level = MAX_TICKET_LEVEL;
            if (SelectedTicket != null) TicketLevelText = MAX_TICKET_LEVEL.ToString();
        }

        [RelayCommand]
        private void ClaimAllCatalogs()
        {
            foreach (var entry in Catalogs) entry.IsClaimed = true;
        }

        [RelayCommand]
        private void MaxProgression()
        {
            if (SelectedGoal == null) return;

            var nextGoal = Goals.FirstOrDefault(g => g.Index == SelectedGoal.Index + 1);
            ProgressionText = nextGoal != null
                ? nextGoal.Goal.ToString(CultureInfo.InvariantCulture)
                : double.MaxValue.ToString(CultureInfo.InvariantCulture);
        }

        partial void OnSearchTicketChanged(string value)
        {
            TicketsView.Refresh();
            OnPropertyChanged(nameof(HasTickets));
        }

        partial void OnSelectedTicketChanged(TicketItem? oldValue, TicketItem? newValue)
        {
            if (oldValue != null) if (!int.TryParse(TicketLevelText, out var level) || level > MAX_TICKET_LEVEL) oldValue.Level = 0;

            TicketLevelText = newValue?.Level.ToString() ?? "0";

            UpdateCurrentTicketImage();
            OnPropertyChanged(nameof(IsTicketSelected));
            OnPropertyChanged(nameof(SelectedTicketToolTip));
        }

        private void UpdateCurrentTicketImage()
        {
            CurrentTicketImage = SelectedTicket?.Ticket!= null
                ? App.ResourceParser.GetSprite(SelectedTicket.Ticket.IconId)
                : Application.Current.TryFindResource("Generic.None") as ImageSource;
        }

        private void UpdateCurrentCatalogImage()
        {
            var temp = SelectedCatalog;
            SelectedCatalog = null;
            SelectedCatalog = temp;
        }

        partial void OnSelectedGoalChanged(ProgressionGoal? value)
        {
            OnPropertyChanged(nameof(IsProgressionValid));
            OnPropertyChanged(nameof(CanSave));
        }
        #endregion

        #region tab upgrades
        [RelayCommand]
        private void MaxPrestige()
        {
            PrestigeText = int.MaxValue.ToString();
        }

        [RelayCommand]
        private void MaxPrestigeUpgradeBuyCountAll()
        {
            foreach (var upgrade in PrestigeUpgrades) upgrade.BuyCountText = upgrade.PrestigeUpgrade != null
                    ? upgrade.PrestigeUpgrade.MaxBuyCount.ToString()
                    : "0";

            if (_deathByFinalChanceCount < 4) _deathByFinalChanceCount = 4;
        }

        [RelayCommand]
        private void MaxPrestigeUpgradeBuyCount(PrestigeUpgradeItem? item)
        {
            if (item == null || item.PrestigeUpgrade == null) return;

            item.BuyCountText = item.PrestigeUpgrade.MaxBuyCount.ToString();

            UpdateDeathCount(item);
        }

        private void UpdateDeathCount(PrestigeUpgradeItem item)
        {
            if (!int.TryParse(item.BuyCountText, out var count) || count <= 0) return;

            switch (item.PrestigeUpgrade?.Id)
            {
                case "Muscle Memory":
                    if (_deathByFinalChanceCount < 1) _deathByFinalChanceCount = 1;
                    break;
                case "Allowance":
                    if (_deathByFinalChanceCount < 2) _deathByFinalChanceCount = 2;
                    break;
                case "Super Lucky":
                    if (_deathByFinalChanceCount < 3) _deathByFinalChanceCount = 3;
                    break;
                case "Time Travel":
                    if (_deathByFinalChanceCount < 4) _deathByFinalChanceCount = 4;
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void MaxUpgradeBuyCountAll()
        {
            foreach (var gadget in Upgrades) gadget.BuyCountText = gadget.Upgrade != null
                    ? gadget.Upgrade.MaxBuyCount.ToString()
                    : "0";
        }

        [RelayCommand]
        private void MaxUpgradeBuyCount(UpgradeItem? item)
        {
            if (item == null || item.Upgrade == null) return;
            item.BuyCountText = item.Upgrade.MaxBuyCount.ToString();
        }

        partial void OnSearchPrestigeUpgradeChanged(string value)
        {
            PrestigeUpgradesView.Refresh();
            OnPropertyChanged(nameof(HasPrestigeUpgrades));
        }

        partial void OnSearchUpgradeChanged(string value)
        {
            UpgradesView.Refresh();
            OnPropertyChanged(nameof(HasUpgrades));
        }
        #endregion

        #region tab achievements
        [RelayCommand]
        private void UnlockAllAchievements()
        {
            foreach (var entry in Achievements) entry.IsUnlocked = true;
        }

        [RelayCommand]
        private void ClaimAllAchievements()
        {
            foreach (var entry in Achievements) entry.IsClaimed = true;
        }

        partial void OnSearchAchievementChanged(string value)
        {
            AchievementsView.Refresh();
            OnPropertyChanged(nameof(HasAchievements));
        }
        #endregion

        #region tab cosmetics
        [RelayCommand]
        private void MaxTokens()
        {
            TokensText = int.MaxValue.ToString();
        }

        [RelayCommand]
        private void UnlockAllCosmetics()
        {
            foreach (var entry in FilteredCosmetics)
            {
                if (entry.Cosmetic == null) continue;
                entry.IsPurchased = true;
            }
        }

        [RelayCommand]
        private void MaxElectricFanCharge()
        {
            ElectricFanChargeText = 1e300.ToString(CultureInfo.InvariantCulture);
        }

        [RelayCommand]
        private void MaxEggTimerCharge()
        {
            EggTimerChargeText = 1e300.ToString(CultureInfo.InvariantCulture);
        }

        partial void OnSelectedCosmeticCategoryChanged(CosmeticCategoryItem? value)
        {
            var filtered = Cosmetics
                .Where(c => c.Cosmetic?.Category == value?.Category)
                .OrderBy(c => c.Cosmetic?.Name)
                .ToList();

            var noneItem = new CosmeticItem
            {
                Cosmetic = null,
                IsPurchased = true,
                IsEquipped = !filtered.Any(c => c.IsEquipped)
            };
            filtered.Insert(0, noneItem);

            FilteredCosmetics = filtered;

            SelectedCosmetic = FilteredCosmetics.FirstOrDefault(c => c.IsEquipped) ?? noneItem;
            //SelectedCosmetic = FilteredCosmetics.FirstOrDefault();
            UpdateCurrentCosmeticImage();
        }

        partial void OnSelectedCosmeticChanged(CosmeticItem? value)
        {
            if (value == null) return;

            IsCosmeticEquipped = value.IsEquipped;
            OnPropertyChanged(nameof(NoneCosmeticNotSelected));
        }

        partial void OnIsCosmeticEquippedChanged(bool value)
        {
            if (SelectedCosmetic == null) return;

            // one item and only one item must always be selected, the last one cannot be deselected
            if (!value && !FilteredCosmetics.Any(c => c != SelectedCosmetic && c.IsEquipped))
            {
                IsCosmeticEquipped = true;
                return;
            }

            if (value)
            {
                foreach (var cosmetic in FilteredCosmetics) cosmetic.IsEquipped = false;
                UpdateCurrentCosmeticImage();
            }

            SelectedCosmetic.IsEquipped = value;
        }

        private void UpdateCurrentCosmeticImage()
        {
            CurrentCosmeticItem = SelectedCosmetic?.Cosmetic != null
                ? App.ResourceParser.GetSprite(SelectedCosmetic.Cosmetic.IconId)
                : Application.Current.TryFindResource("Generic.None") as ImageSource;
        }
        #endregion

        #region tab misc
        [RelayCommand]
        private void UnlockAllDlcs()
        {
            foreach (var entry in Dlcs) entry.IsUnlocked = true;
        }
        #endregion
    }
}
