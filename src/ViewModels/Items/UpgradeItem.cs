using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class UpgradeItem : ObservableObject
    {
        public Upgrade? Upgrade { get; init; }

        [ObservableProperty]
        private string _buyCountText = "0";

        public bool IsMaxedOut => int.TryParse(BuyCountText, out var value)
            && value == Upgrade?.MaxBuyCount;
        public bool IsBuyCountValid => int.TryParse(BuyCountText, out var value)
            && value >= 0
            && value <= (Upgrade?.MaxBuyCount ?? 0);

        partial void OnBuyCountTextChanged(string value)
        {
            OnPropertyChanged(nameof(IsBuyCountValid));
            OnPropertyChanged(nameof(IsMaxedOut));
        }
    }
}
