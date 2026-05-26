using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScritchyScratchyCheater.ViewModels.Pages.EditorV01
{
    internal partial class EditorV01ViewModel : ObservableObject
    {
        #region Tab Progress
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsMoneyValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _moneyText = string.Empty;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSoulsValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _soulsText = string.Empty;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTicketLevelValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _ticketLevelText = "0";
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsMachineTierValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _machineTierText = string.Empty;

        [ObservableProperty]
        private ImageSource? _moneyIcon;
        [ObservableProperty]
        private ImageSource? _soulsIcon;
        [ObservableProperty]
        private ImageSource? _starIcon;
        [ObservableProperty]
        private ImageSource? _cyanStarIcon;
        [ObservableProperty]
        private ImageSource? _theMachine;
        [ObservableProperty]
        private ImageSource? _currentTicketImage = (ImageSource)Application.Current.TryFindResource("Generic.None");

        private const int MAX_TICKET_LEVEL = int.MaxValue;
        private const int MAX_MACHINE_TIER = 26;

        public bool IsMoneyValid =>
            double.TryParse(MoneyText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            && !double.IsNaN(value)
            && !double.IsInfinity(value);
        public bool IsSoulsValid => int.TryParse(SoulsText, out _);
        public bool IsTicketLevelValid => int.TryParse(TicketLevelText, out var value)
            && value <= MAX_TICKET_LEVEL;
        public bool IsMachineTierValid => int.TryParse(MachineTierText, out var value)
            && value <= MAX_MACHINE_TIER;

        public bool IsTicketSelected => SelectedTicket != null;

        [ObservableProperty]
        private string _searchTicket = string.Empty;
        public ObservableCollection<TicketItem> Tickets { get; } = new();
        public ICollectionView TicketsView { get; }
        public bool HasTickets => !TicketsView.IsEmpty;

        [ObservableProperty]
        private TicketItem? _selectedTicket;

        public string? SelectedTicketToolTip => SelectedTicket == null
            ? null
            : $"Ticket: '{SelectedTicket.Ticket!.Name}'";

        public ObservableCollection<CatalogItem> Catalogs { get; } = new();
        [ObservableProperty]
        private CatalogItem? _selectedCatalog;

        public IReadOnlyList<Act> Acts { get; } = new List<Act>
        {
            new() { Name = "Act 1", ActId = 1 },
            new() { Name = "Act 2", ActId = 2 },
            new() { Name = "Act 3", ActId = 3 },
            new() { Name = "Act 4", ActId = 4 },
            new() { Name = "Act 5", ActId = 5 }
        }.OrderBy(c => c.Name).ToList();

        [ObservableProperty]
        private Act? _selectedAct;
        #endregion

        #region Tab Upgrades
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPrestigeValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _prestigeText = string.Empty;

        public bool IsPrestigeValid => int.TryParse(PrestigeText, out _);

        [ObservableProperty]
        private ImageSource? _prestigeIcon;
        #endregion

        #region Tab Achivements
        [ObservableProperty]
        private string _searchAchievement = string.Empty;
        public ObservableCollection<AchievementItem> Achievements { get; } = new();
        public ICollectionView AchievementsView { get; }
        public bool HasAchievements => !AchievementsView.IsEmpty;
        #endregion

        #region Tab Cosmetics
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTokensValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _tokensText = string.Empty;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEelectricFanChargeValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _electricFanChargeText = string.Empty;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEggTimerChargeValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _eggTimerChargeText = string.Empty;

        [ObservableProperty]
        private ImageSource? _tokensIcon;
        [ObservableProperty]
        private ImageSource? _electricFanIcon;
        [ObservableProperty]
        private ImageSource? _eggTimerIcon;
        [ObservableProperty]
        private ImageSource? _mundoIcon;
        [ObservableProperty]
        private ImageSource? _trashCanIcon;

        public bool IsTokensValid => int.TryParse(TokensText, out _);
        public bool NoneCosmeticNotSelected => SelectedCosmetic?.Cosmetic != null;
        public bool IsEelectricFanChargeValid =>
            double.TryParse(ElectricFanChargeText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            && !double.IsNaN(value)
            && !double.IsInfinity(value);
        public bool IsEggTimerChargeValid =>
            double.TryParse(EggTimerChargeText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            && !double.IsNaN(value)
            && !double.IsInfinity(value);

        public IReadOnlyList<CosmeticCategoryItem> CosmeticCategories { get; } = new List<CosmeticCategoryItem>
        {
            new() { Category = CosmeticCategory.Phone, Name = "Phones" },
            new() { Category = CosmeticCategory.Table, Name = "Tables" },
            new() { Category = CosmeticCategory.TrashCan, Name = "Trash Cans" },
            new() { Category = CosmeticCategory.ElectricFan, Name = "Electric Fans" },
            new() { Category = CosmeticCategory.Mundo, Name = "Mundos" },
            new() { Category = CosmeticCategory.EggTimer, Name = "Egg Timers" },
            new() { Category = CosmeticCategory.ScratchBot, Name = "Scratch Bots" },
        }.OrderBy(c => c.Name).ToList();

        [ObservableProperty]
        private CosmeticCategoryItem? _selectedCosmeticCategory;

        public ObservableCollection<CosmeticItem> Cosmetics { get; } = new();

        [ObservableProperty]
        private IEnumerable<CosmeticItem> _filteredCosmetics = [];

        [ObservableProperty]
        private CosmeticItem? _selectedCosmetic;

        [ObservableProperty]
        private bool _isCosmeticEquipped;

        [ObservableProperty]
        private ImageSource? _currentCosmeticItem;

        [ObservableProperty]
        private bool _isElectricFanPaused;
        [ObservableProperty]
        private bool _isMundoDead;
        [ObservableProperty]
        private bool _isTrashCanDead;
        #endregion
    }
}
