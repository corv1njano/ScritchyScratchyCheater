using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using ScritchyScratchyCheater.Utilities;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

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
        public event Action<string>? SaveFileVersionChanged;

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

            if (!File.Exists(filePath))
            {
                ShowMessage.Error("File not found",
                    "Unable to find the selected file or the default file. It may have been renamed, removed or deleted.",
                    DialogOptions.Ok);
                return (false, null);
            }
            if (new FileInfo(filePath).Length == 0)
            {
                ShowMessage.Error("File empty",
                    "The selected file does not contain any data. Cannot open the save file.",
                    DialogOptions.Ok);
                return (false, null);
            }

            var json = await File.ReadAllTextAsync(filePath);

            SaveFileVersionInfo? versionInfo;
            try
            {
                versionInfo = JsonSerializer.Deserialize<SaveFileVersionInfo>(json, App.JsonOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                ShowMessage.Error("Invalid save file",
                    "Could not read the save file. The data structure is invalid.",
                    DialogOptions.Ok);
                return (false, null);
            }

            if (versionInfo?.SaveVersion == null || !App.SupportedVersions.Contains(versionInfo.SaveVersion))
            {
                ShowMessage.Error("Invalid save version",
                    "The save version is not supported. Cannot edit the save file.",
                    DialogOptions.Ok);
                return (false, null);
            }

            ISaveFile? loadedSave;
            try
            {
                loadedSave = versionInfo.SaveVersion switch
                {
                    "0.1" => JsonSerializer.Deserialize<SaveFileV01>(json, App.JsonOptions),
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                ShowMessage.Error("Invalid save file",
                    "Could not read the save file. The data structure is invalid.",
                    DialogOptions.Ok);
                return (false, null);
            }

            if (loadedSave == null)
            {
                ShowMessage.Error("Invalid save file",
                    "The save file could not be parsed. It may have an unsupported version or an invalid data structure.",
                    DialogOptions.Ok);
                return (false, null);
            }

            LoadedSaveFile = loadedSave;
            SaveFileChanged?.Invoke(loadedSave);
            Debug.WriteLine(LoadedSaveFile.ToJson());

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

        /// <summary>
        /// Resets the current save file state, clearing all loaded data and file information.
        /// </summary>
        public void Reset()
        {
            if (LoadedSaveFile == null && string.IsNullOrEmpty(CurrentFilePath)) return;

            LoadedSaveFile = null;
            CurrentFilePath = string.Empty;
            CurrentSaveFileVersion = string.Empty;

            SaveFileChanged?.Invoke(LoadedSaveFile);
            FilePathChanged?.Invoke(CurrentFilePath);
            SaveFileVersionChanged?.Invoke(CurrentSaveFileVersion);
        }
    }
}
