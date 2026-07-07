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
        private int _level; // TODO: move level validation in this class (see LoanItem.cs)

        [ObservableProperty]
        private int _xp;
    }
}
