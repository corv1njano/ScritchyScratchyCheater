using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ScritchyScratchyCheater.Services
{
    public class SaveFileService
    {
        public string DefaultSaveFilePath { get; private set; }

        public ISaveFile? LoadedSaveFile { get; private set; } = null;
        public event Action<ISaveFile?>? SaveFileChanged;

        public string CurrentFilePath { get; private set; } = string.Empty;
        public event Action<string>? FilePathChanged;
        public string CurrentSaveFileVersion { get; private set; } = string.Empty;

        public SaveFileService()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string localLow = Path.Combine(Directory.GetParent(localAppData)!.FullName, "LocalLow");
            DefaultSaveFilePath = Path.Combine(
                localLow,
                "Lunch Money Games",
                "Scritchy Scratchy",
                "save.json"
            );
        }

        public async Task<(bool success, string? version)> Initialize(string filePath)
        {
            Reset();

            if (!File.Exists(filePath)) return (false, null);
            if (new FileInfo(filePath).Length == 0) return (false, null);

            var json = await File.ReadAllTextAsync(filePath);
            json = SanitizeJsonNumbers(json);

            SaveFileVersionInfo? versionInfo;
            ISaveFile? loadedSave;

            try
            {
                versionInfo = JsonSerializer.Deserialize<SaveFileVersionInfo>(json, App.JsonOptions);

                if (versionInfo?.SaveVersion == null) return (false, null);

                loadedSave = versionInfo.SaveVersion switch
                {
                    "0.1" => JsonSerializer.Deserialize<SaveFileV01>(json, App.JsonOptions),
                    _ => null
                };
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Save file deserialization failed: {ex}");
                return (false, null);
            }

            if (loadedSave == null) return (false, null);

            LoadedSaveFile = loadedSave;
            SaveFileChanged?.Invoke(loadedSave);
            //Debug.WriteLine(LoadedSaveFile.ToJson());

            CurrentFilePath = filePath;
            FilePathChanged?.Invoke(CurrentFilePath);
            CurrentSaveFileVersion = versionInfo.SaveVersion;

            return (true, versionInfo.SaveVersion);
        }

        public async Task<bool> Save()
        {
            if (LoadedSaveFile == null) return false;
            if (string.IsNullOrWhiteSpace(CurrentFilePath)) return false;

            try
            {
                string json = LoadedSaveFile.ToJson();
                await File.WriteAllTextAsync(CurrentFilePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Saveing failed: {ex}");
                return false;
            }
        }

        public void Reset()
        {
            if (LoadedSaveFile == null && string.IsNullOrEmpty(CurrentFilePath)) return;

            LoadedSaveFile = null;
            CurrentFilePath = string.Empty;
            CurrentSaveFileVersion = string.Empty;

            SaveFileChanged?.Invoke(LoadedSaveFile);
            FilePathChanged?.Invoke(CurrentFilePath);
        }

        /// <summary>
        /// Replaces invalid JSON number literals (<c>NaN</c>, <c>Infinity</c>, <c>-Infinity</c>)
        /// with <c>0</c> so that the JSON can be deserialized without errors.
        /// </summary>
        private static string SanitizeJsonNumbers(string json)
        {
            // NaN and ±Infinity are not valid JSON literals; replace them with 0.
            // The lookbehind/lookahead guards ensure only unquoted numeric values are replaced,
            // not occurrences inside JSON string values.
            return Regex.Replace(json, @"(?<![""a-zA-Z0-9_])-?(?:NaN|Infinity)(?![""a-zA-Z0-9_])", "0");
        }
    }
}
