using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        private ImageSource? _moneyIcon;
        [ObservableProperty]
        private ImageSource? _soulsIcon;

        public bool IsMoneyValid =>
            double.TryParse(MoneyText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            && !double.IsNaN(value)
            && !double.IsInfinity(value);
        public bool IsSoulsValid => int.TryParse(SoulsText, out _);
        #endregion

        #region Tab Prestige
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
        public bool HasEntries => !AchievementsView.IsEmpty;
        #endregion

        #region Tab Cosmetics
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTokensValid))]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private string _tokensText = string.Empty;

        [ObservableProperty]
        private ImageSource? _tokensIcon;

        public bool IsTokensValid => int.TryParse(TokensText, out _);

        public IReadOnlyList<CosmeticCategoryItem> CosmeticCategories { get; } = new List<CosmeticCategoryItem>
        {
            new() { Category = CosmeticCategory.Phone, Name = "Phones" },
            new() { Category = CosmeticCategory.Table, Name = "Tables" },
            new() { Category = CosmeticCategory.TrashCan, Name = "Trash Cans" },
            new() { Category = CosmeticCategory.Fan, Name = "Fans" },
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
        #endregion
    }
}
