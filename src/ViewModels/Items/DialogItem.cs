using CommunityToolkit.Mvvm.ComponentModel;

namespace ScritchyScratchyCheater.ViewModels.Items
{
    public sealed partial class DialogItem : ObservableObject
    {
        /// <summary>
        /// Internal ID for a dialog entry but also the display name for UI.
        /// </summary>
        public string? DialogId { get; init; }

        [ObservableProperty]
        private bool _played;

        public override string ToString()
        {
            return DialogId != null ? DialogId : "Unknown dialog";
        }
    }
}
