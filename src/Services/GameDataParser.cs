using ScritchyScratchyCheater.Models.GameData;
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
        /// Loads and provides game data from the game data JSON resource.
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
        /// Retrieves a list of cosmetic items from the current game data.
        /// </summary>
        /// <returns>A list of <see cref="Cosmetic"/> objects representing the comsetic item associated with the game data.</returns>
        public List<Cosmetic> GetCosmetics()
        {
            var cosmetics = GetDataSet<Cosmetic>("cosmetics");

            foreach (Cosmetic entry in cosmetics)
            {
                entry.Category = entry.Category switch
                {
                    CosmeticCategory.Phone       => CosmeticCategory.Phone,
                    CosmeticCategory.Table       => CosmeticCategory.Table,
                    CosmeticCategory.TrashCan    => CosmeticCategory.TrashCan,
                    CosmeticCategory.ElectricFan => CosmeticCategory.ElectricFan,
                    CosmeticCategory.Mundo       => CosmeticCategory.Mundo,
                    CosmeticCategory.EggTimer    => CosmeticCategory.EggTimer,
                    CosmeticCategory.ScratchBot  => CosmeticCategory.ScratchBot,

                    // this fallback should never happen, as "Other" category will not appear in the UI
                    _ => CosmeticCategory.Other
                };
            }

            return cosmetics;
        }

        /// <summary>
        /// Gets a data set by name from the game data JSON file.
        /// </summary>
        /// <typeparam name="T">The type of the data object to parse to.</typeparam>
        /// <param name="dataSetName">The name of the data set type.</param>
        /// <returns>A list with all game objects by the given data set group.</returns>
        public List<T> GetDataSet<T>(string dataSetName) where T : class
        {
            if (_gameDataRoot == null || string.IsNullOrWhiteSpace(dataSetName)) return new();

            GameDataset? dataset = _gameDataRoot.GameData?
                .FirstOrDefault(x => string.Equals(x.Type, dataSetName, StringComparison.OrdinalIgnoreCase));

            if (dataset == null) return new();
            if (dataset.Data.ValueKind == JsonValueKind.Undefined || dataset.Data.ValueKind == JsonValueKind.Null) return new();

            return dataset.Data.Deserialize<List<T>>(App.JsonOptions) ?? new();
        }
    }

    #region game data containers
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
    #endregion
}
