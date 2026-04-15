using ScritchyScratchyCheater.Services;
using System.Text.Json;
using System.Windows;

namespace ScritchyScratchyCheater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static JsonSerializerOptions JsonOptions { get; } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static SaveFileService SaveFileService { get; } = new();
        public static ResourceParser ResourceParser { get; } = new();
        public static PageNavigator PageNavigator { get; } = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
