using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Models.GameData;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ScritchyScratchyCheater.ViewModels.Pages.EditorV01
{
    internal partial class EditorV01ViewModel : ObservableObject
    {
        #region Tab Progress
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

        partial void OnSearchTicketChanged(string value)
        {
            TicketsView.Refresh();
            OnPropertyChanged(nameof(HasTickets));
        }

        partial void OnSelectedTicketChanged(TicketItem? oldValue, TicketItem? newValue)
        {
            if (oldValue != null) if (!int.TryParse(TicketLevelText, out var level) || level > MAX_TICKET_LEVEL) oldValue.Level = 0;

            TicketLevelText = newValue?.Level.ToString() ?? "0";
        }
        #endregion

        #region Tab Prestige
        [RelayCommand]
        private void MaxPrestige()
        {
            PrestigeText = int.MaxValue.ToString();
        }
        #endregion

        #region Tab Achievements
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

        #region Tab Cosmetics
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
                : Application.Current.TryFindResource("Cosmetic.None") as ImageSource;
        }
        #endregion
    }
}
