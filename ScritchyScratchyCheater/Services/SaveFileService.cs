using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

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
            SaveFileVersionInfo? versionInfo = JsonSerializer.Deserialize<SaveFileVersionInfo>(json, App.JsonOptions);

            if (versionInfo?.SaveVersion == null) return (false, null);

            ISaveFile? loadedSave = versionInfo.SaveVersion switch
            {
                "0.1" => JsonSerializer.Deserialize<SaveFileV01>(json, App.JsonOptions),
                _ => null
            };

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
    }
}
