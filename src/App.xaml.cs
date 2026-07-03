using ScritchyScratchyCheater.Services;
using System.Reflection;
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
        /// <summary>
        /// Json parser options for save file data and internally used data sets.
        /// </summary>
        public static JsonSerializerOptions JsonOptions { get; } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        /// <summary>
        /// Current app version. Used for update check in <see cref="Utilities.UpdateChecker"/>, UI version
        /// display in <see cref="MainWindow"/> and <see cref="Views.Dialogs.AboutAppDialog"/>.
        /// </summary>
        public static readonly string APP_VERSION =
            Assembly.GetExecutingAssembly().GetName().Version is { } v
                ? $"v{v.Major}.{v.Minor}.{v.Build}"
                : string.Empty;

        /// <summary>
        /// List with all save file versions to be checked for support. Only uf present it will be supported.
        /// Not supported versions cannot be edited.
        /// </summary>
        public static readonly List<string> SupportedVersions = new()
        {
            "0.1",
        };

        // initialize lifetime services for app-wide useage
        public static PageNavigator PageNavigator { get; } = new();
        public static SaveFileService SaveFileService { get; } = new();
        public static ResourceParser ResourceParser { get; } = new();
        public static GameDataParser GameDataParser { get; } = new();
    }
}
