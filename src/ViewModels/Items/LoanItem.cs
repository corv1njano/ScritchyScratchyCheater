using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class LoanItem : ObservableObject
    {
        public Loan? Loan { get; init; }

        [ObservableProperty]
        private int _index;

        [ObservableProperty]
        private int _loanNum;

        [ObservableProperty]
        private int _servity = 1;

        [ObservableProperty]
        private double _amount = 1;

        public string LoanIdPretty => "#" + LoanNum;
        public string LoanDescriptionPretty => string.Format(Loan!.Description, Loan.Levels[Servity - 1]);
    }
}
