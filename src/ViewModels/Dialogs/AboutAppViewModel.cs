using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScritchyScratchyCheater.Utilities;

namespace ScritchyScratchyCheater.ViewModels.Dialogs
{
    internal partial class AboutAppViewModel : ObservableObject
    {
        public string AppNameAndVersion { get; }

        public AboutAppViewModel()
        {
            AppNameAndVersion = $"Scritchy Srcatchy Cheater {App.APP_VERSION}";
        }

        [RelayCommand]
        private void OpenGitHub()
        {
            Utils.OpenUrl("https://github.com/corv1njano/ScritchyScratchyCheater");
        }
        [RelayCommand]
        private void OpenLicense()
        {
            Utils.OpenUrl("https://github.com/corv1njano/ScritchyScratchyCheater/blob/master/LICENSE.txt");
        }
        [RelayCommand]
        private void OpenSponsor()
        {
            Utils.OpenUrl("https://github.com/sponsors/corv1njano/");
        }
    }
}
