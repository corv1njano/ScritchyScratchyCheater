using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Data
{
    public sealed partial class AchievementItem : ObservableObject
    {
        public Achievement? Achievement { get; init; }

        [ObservableProperty]
        private bool _isUnlocked;

        [ObservableProperty]
        private bool _isClaimed;
    }
}
