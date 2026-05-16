using System.IO;
using System.Text.Json;
using System.Windows;

namespace ScritchyScratchyCheater.Services
{
    /// <summary>
    /// Provides functionality for loading and accessing game data from the embedded JSON resource.
    /// </summary>
    public class GameDataParser
    {
        private const string GameDataResourcePath = "pack://application:,,,/Resources/Data/GameData.json";

        private GameDataRoot? _gameDataRoot;

        public GameDataParser()
        {
            LoadGameData();
        }

        /// <summary>
        /// Loads and provides game data from the game_data.json resource.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the game data could not be loaded or deserialized.</exception>
        public void LoadGameData()
        {
            var uri = new Uri(GameDataResourcePath, UriKind.Absolute);
            var resourceInfo = Application.GetResourceStream(uri);

            if (resourceInfo?.Stream == null) throw new InvalidOperationException("game_data.json could not be read.");

            using var reader = new StreamReader(resourceInfo.Stream);
            string json = reader.ReadToEnd();

            _gameDataRoot = JsonSerializer.Deserialize<GameDataRoot>(json, App.JsonOptions);

            if (_gameDataRoot == null) throw new InvalidOperationException("Deserialization failed.");
        }

        /// <summary>
        /// Retrieves a list of achievements from the current game data.
        /// </summary>
        /// <remarks>This method returns an empty list if the game data root is null, if no achievement
        /// dataset is present, or if the achievement data is undefined or null.</remarks>
        /// <returns>A list of <see cref="Achievement"/> objects representing the achievements associated with the game data.</returns>
        public List<Achievement> GetAchievements()
        {
            if (_gameDataRoot == null) return new();

            GameDataset? dataset = _gameDataRoot.GameData?
                .FirstOrDefault(x => string.Equals(x.Type, "achievements", StringComparison.OrdinalIgnoreCase));

            if (dataset == null) return new();
            if (dataset.Data.ValueKind == JsonValueKind.Undefined || dataset.Data.ValueKind == JsonValueKind.Null) return new();

            List<Achievement>? achievements = dataset.Data.Deserialize<List<Achievement>>(App.JsonOptions);

            return achievements!
                .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ToList() 
                ?? new();
        }
    }

    #region game data
    public sealed class GameDataRoot
    {
        public string SaveVersion { get; set; } = string.Empty;
        public string GameVersion { get; set; } = string.Empty;
        public List<GameDataset>? GameData { get; set; }
    }

    public sealed class GameDataset
    {
        public string Type { get; set; } = string.Empty;
        public JsonElement Data { get; set; }
    }

    public sealed class Achievement
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string IconId { get; set; } = string.Empty;
    }
    #endregion
}
