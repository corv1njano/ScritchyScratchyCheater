using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;
using ScritchyScratchyCheater.ViewModels.Items;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ScritchyScratchyCheater.ViewModels.Pages.EditorV01
{
    public partial class EditorV01ViewModel : ObservableObject
    {
        #region tab progress
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
        [NotifyPropertyChangedFor(nameof(IsProgressionValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _progressionText = "0";

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
        public bool IsProgressionValid => SelectedGoal != null
            && double.TryParse(ProgressionText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            && !double.IsNaN(value)
            && !double.IsInfinity(value)
            && value >= 0;
            //&& value <= (Goals.FirstOrDefault(g => g.Index == SelectedGoal.Index + 1)?.Goal ?? double.MaxValue);

        public bool IsTicketSelected => SelectedTicket != null;

        [ObservableProperty]
        private string _searchTicket = string.Empty;
        public ObservableCollection<TicketItem> Tickets { get; } = new();
        public ICollectionView TicketsView { get; }
        public bool HasTickets => !TicketsView.IsEmpty;

        [ObservableProperty]
        private TicketItem? _selectedTicket;

        public string? SelectedTicketToolTip => SelectedTicket == null || SelectedTicket.Ticket == null
            ? null
            : $"Ticket: '{SelectedTicket.Ticket.Name}'";

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
        }.OrderBy(i => i.Name).ToList();

        [ObservableProperty]
        private Act? _selectedAct;

        public IReadOnlyList<ProgressionGoal> Goals { get; } = new List<ProgressionGoal>
        {
            new() { Index = 0, Goal = 0 },
            new() { Index = 1, Goal = 3 },
            new() { Index = 2, Goal = 10 },
            new() { Index = 3, Goal = 50 },
            new() { Index = 4, Goal = 100 },
            new() { Index = 5, Goal = 2000 },
            new() { Index = 6, Goal = 10000 },
            new() { Index = 7, Goal = 50000 },
            new() { Index = 8, Goal = 100000 },
            new() { Index = 9, Goal = 300000 },
            new() { Index = 10, Goal = 2e7 },
            new() { Index = 11, Goal = 1e8 },
            new() { Index = 12, Goal = 5e8 },
            new() { Index = 13, Goal = 2e9 },
            new() { Index = 14, Goal = 1e10 },
            new() { Index = 15, Goal = 2e11 },
            new() { Index = 16, Goal = 2e14 },
            new() { Index = 17, Goal = 1e16 },
            new() { Index = 18, Goal = 1e17 },
            new() { Index = 19, Goal = 5e17 },
            new() { Index = 20, Goal = 2e19 },
            new() { Index = 21, Goal = 1e23 },
            new() { Index = 22, Goal = 1e24 },
            new() { Index = 23, Goal = 5e24 },
            new() { Index = 24, Goal = 8e26 },
            new() { Index = 25, Goal = 3e28 },
            new() { Index = 26, Goal = 1e30 }
        };//.OrderBy(i => i.Index).ToList();

        [ObservableProperty]
        private ProgressionGoal? _selectedGoal;
        #endregion

        #region tab upgrades
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPrestigeValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _prestigeText = string.Empty;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPrestigeCountValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _prestigeCountText = string.Empty;

        public bool IsPrestigeValid => int.TryParse(PrestigeText, out _);
        public bool IsPrestigeCountValid => int.TryParse(PrestigeCountText, out _);

        [ObservableProperty]
        private ImageSource? _prestigeIcon;

        [ObservableProperty]
        private string _searchPrestigeUpgrade = string.Empty;
        public ObservableCollection<PrestigeUpgradeItem> PrestigeUpgrades { get; } = new();
        public ICollectionView PrestigeUpgradesView { get; }
        public bool HasPrestigeUpgrades => !PrestigeUpgradesView.IsEmpty;

        [ObservableProperty]
        private string _searchUpgrade = string.Empty;
        public ObservableCollection<UpgradeItem> Upgrades { get; } = new();
        public ICollectionView UpgradesView { get; }
        public bool HasUpgrades => !UpgradesView.IsEmpty;

        private int _deathByFinalChanceCount;
        #endregion

        #region tab achivements
        [ObservableProperty]
        private string _searchAchievement = string.Empty;
        public ObservableCollection<AchievementItem> Achievements { get; } = new();
        public ICollectionView AchievementsView { get; }
        public bool HasAchievements => !AchievementsView.IsEmpty;
        #endregion

        #region tab cosmetics
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

        public string? CurrentCosmeticItemToolTip
        {
            get
            {
                var item = FilteredCosmetics.FirstOrDefault(c => c.IsEquipped);
                return item?.Cosmetic == null
                    ? null
                    : $"Current Item: '{item.Cosmetic.Name}'";
            }
        }

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

        #region tab misc
        public ObservableCollection<DlcItem> Dlcs { get; } = new();
        [ObservableProperty]
        private DlcItem? _selectedDlc;
        #endregion
    }
}
