using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;
using System.Collections.ObjectModel;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class LoanItem : ObservableObject
    {
        /// <summary>
        /// List of all available loans from the game data JSON.
        /// </summary>
        public ObservableCollection<Loan> LoanIds { get; }
        public static ObservableCollection<LoanServity> LoanServities { get; } = new()
        {
            new LoanServity { Name = "Level 1", Servity = 1 },
            new LoanServity { Name = "Level 2", Servity = 2 },
            new LoanServity { Name = "Level 3", Servity = 3 },
        };

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LoanDescriptionPretty))]
        private Loan? _loan;

        [ObservableProperty]
        private int _index;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LoanIdPretty))]
        private int _loanNum;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LoanDescriptionPretty))]
        private int _servity = 1;

        [ObservableProperty]
        private double _amount = 1;

        public string LoanIdPretty => "#" + LoanNum;
        public string LoanDescriptionPretty =>
            Loan is null
                ? string.Empty
                : string.Format(Loan.Description, Loan.Levels[Servity - 1]);

        public LoanItem(ObservableCollection<Loan> availableLoans, Loan? initialLoan = null)
        {
            LoanIds = availableLoans;
            Loan = initialLoan ?? availableLoans.FirstOrDefault();
        }
    }
}