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
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public const string APP_VERSION = "v1.3.0";

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
