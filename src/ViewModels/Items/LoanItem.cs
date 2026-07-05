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
        public ObservableCollection<Loan> AvailableLoans { get; init; }
        public static ObservableCollection<LoanSeverity> LoanSeverities { get; } = new()
        {
            new LoanSeverity { Name = "Level 1", Severity = 1 },
            new LoanSeverity { Name = "Level 2", Severity = 2 },
            new LoanSeverity { Name = "Level 3", Severity = 3 },
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
        private int _severity = 1;

        [ObservableProperty]
        private double _amount = 1;

        public string LoanIdPretty => "#" + LoanNum;
        public string LoanDescriptionPretty =>
            Loan == null
                ? string.Empty
                : string.Format(Loan.Description, Loan.Levels[Severity - 1]);

        /// <summary>
        /// Called when a new loan entry is created by the user.
        /// </summary>
        public LoanItem(ObservableCollection<Loan> availableLoans, Loan? initialLoan = null)
        {
            AvailableLoans = availableLoans;
            Loan = initialLoan ?? availableLoans.FirstOrDefault();
        }

        /// <summary>
        /// Called when new loan entry is loaded from the save file into the editor.
        /// </summary>
        public LoanItem(Loan? loan, int index, int loanNum, int severity, double amount, ObservableCollection<Loan> availableLoans)
        {
            Loan = loan;
            Index = index;
            LoanNum = loanNum;
            Severity = severity;
            Amount = amount;
            AvailableLoans = availableLoans;
        }
    }
}