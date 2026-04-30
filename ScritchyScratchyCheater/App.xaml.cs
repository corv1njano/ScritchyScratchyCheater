using ScritchyScratchyCheater.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        public const string APP_VERSION = "v1.2.1";

        public static readonly HashSet<string> SupportedVersions = new()
        {
            "0.1",
        };

        public static PageNavigator PageNavigator { get; } = new();
        public static SaveFileService SaveFileService { get; } = new();
        public static ResourceParser ResourceParser { get; } = new();
        public static GameDataParser GameDataParser { get; } = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
