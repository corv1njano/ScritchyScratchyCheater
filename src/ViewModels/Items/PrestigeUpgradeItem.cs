using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class PrestigeUpgradeItem : ObservableObject
    {
        public PrestigeUpgrade? PrestigeUpgrade { get; init; }

        [ObservableProperty]
        private string _buyCountText = "0";

        public bool IsMaxedOut => int.TryParse(BuyCountText, out var value)
            && value == PrestigeUpgrade?.MaxBuyCount;
        public bool IsBuyCountValid => int.TryParse(BuyCountText, out var value)
            && value >= 0
            && value <= (PrestigeUpgrade?.MaxBuyCount ?? 0);

        partial void OnBuyCountTextChanged(string value)
        {
            OnPropertyChanged(nameof(IsBuyCountValid));
            OnPropertyChanged(nameof(IsMaxedOut));
        }
    }
}
