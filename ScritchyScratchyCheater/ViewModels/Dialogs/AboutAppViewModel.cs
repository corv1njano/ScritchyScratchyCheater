using CommunityToolkit.Mvvm.ComponentModel;

namespace ScritchyScratchyCheater.ViewModels.Dialogs
{
    internal partial class AboutAppViewModel : ObservableObject
    {
        public string AppNameAndVersion { get; }
        //public string LicenseText { get; }

        public AboutAppViewModel()
        {
            AppNameAndVersion = $"Scritchy Srcatchy Cheater {App.APP_VERSION}";

            //try
            //{
            //    LicenseText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/gpl-3.0.txt"));
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"Could not load license: {ex}");

            //    LicenseText = "The license could not be loaded. Please check the official GitHub page for the license information.";
            //}
        }
    }
}
