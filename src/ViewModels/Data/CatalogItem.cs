using CommunityToolkit.Mvvm.ComponentModel;
using ScritchyScratchyCheater.Models.GameData;

namespace ScritchyScratchyCheater.ViewModels.Data
{
    public sealed partial class CatalogItem : ObservableObject
    {
        public Catalog? Catalog { get; init; }

        [ObservableProperty]
        private bool _isClaimed;
    }
}
