using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class TicketItem : ObservableObject
    {
        public Ticket? Ticket { get; init; }

        [ObservableProperty]
        private bool _gottenJackpot;

        [ObservableProperty]
        private bool _gottenSuperJackpot;

        [ObservableProperty]
        private int _level; // check if int

        [ObservableProperty]
        private int _xp; // check if int
    }
}
