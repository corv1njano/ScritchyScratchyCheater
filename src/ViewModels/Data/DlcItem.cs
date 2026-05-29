using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;
using System.Xml.Linq;

namespace ScritchyScratchyCheater.ViewModels.Data
{
    public sealed partial class DlcItem : ObservableObject
    {
        public Dlc? Dlc { get; init; }

        [ObservableProperty]
        private bool _isUnlocked;

        public override string ToString() { return Dlc != null ? Dlc.Name : "Unknown DLC"; }
    }
}
