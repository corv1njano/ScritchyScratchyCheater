using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;
using System.Globalization;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class TicketItem : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLevelValid))]
        private string _levelText = string.Empty;

        public Ticket? Ticket { get; init; }

        [ObservableProperty]
        private bool _gottenJackpot;

        [ObservableProperty]
        private bool _gottenSuperJackpot;

        [ObservableProperty]
        private int _level;

        [ObservableProperty]
        private int _xp;

        public bool IsLevelValid => int.TryParse(LevelText, out var value)
            && value >= 0;

        partial void OnLevelTextChanged(string value)
        {
            if (int.TryParse(value, out var parsed) && parsed >= 0)
            {
                Level = parsed;
            }
        }
    }
}
