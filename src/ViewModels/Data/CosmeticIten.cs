using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Data
{
    public sealed partial class CosmeticItem : ObservableObject
    {
        public Cosmetic? Cosmetic { get; init; }

        [ObservableProperty]
        private bool _isPurchased;

        [ObservableProperty]
        private bool _isEquipped;

        public string DisplayName => Cosmetic?.Name ?? "None (default)";
    }
}
