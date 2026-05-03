using ScritchyScratchyCheater.Interfaces;
using ScritchyScratchyCheater.SaveFiles;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ScritchyScratchyCheater.Services
{
    /// <summary>
    /// Provides functionality for loading, saving, and managing the application's save file.
    /// </summary>
    public class SaveFileService
    {
        public string DefaultSaveFilePath { get; init; }

        public ISaveFile? LoadedSaveFile { get; private set; }
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

        /// <summary>
        /// Loads a save file from the specified path, validates its structure and version, and updates the current
        /// application state accordingly.
        /// </summary>
        /// <param name="filePath">The path to the save file to load. Must refer to an existing, non-empty file containing a supported save
        /// file format.</param>
        /// <returns>A LoadResult indicating whether the operation succeeded, the detected save file version if successful, and a
        /// status message describing the outcome.</returns>
        public async Task<LoadResult> Load(string filePath)
        {
            Reset();

            if (!File.Exists(filePath))
            {
                return new LoadResult()
                {
                    Success = false,
                    StatusMessage = "Unable to find the selected file or the default file. It may have been renamed, removed or deleted."
                };
            }
            if (new FileInfo(filePath).Length == 0)
            {
                return new LoadResult()
                {
                    Success = false,
                    StatusMessage = "The selected file does not contain any data. Cannot open the save file."
                };
            }

            var json = await File.ReadAllTextAsync(filePath);

            SaveFileVersionInfo? versionInfo;
            try
            {
                versionInfo = JsonSerializer.Deserialize<SaveFileVersionInfo>(json, App.JsonOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading failed: {ex}");

                return new LoadResult()
                {
                    Success = false,
                    StatusMessage = "Could not read the save file. The data structure is invalid."
                };
            }

            var version = versionInfo?.SaveVersion;
            if (string.IsNullOrWhiteSpace(version) ||
                !App.SupportedVersions.Contains(version))
            {
                return new LoadResult()
                {
                    Success = false,
                    StatusMessage = "The save version is not supported. Cannot edit the save file."
                };
            }

            ISaveFile? loadedSave;
            try
            {
                loadedSave = version switch
                {
                    "0.1" => JsonSerializer.Deserialize<SaveFileV01>(json, App.JsonOptions),
                    _ => null
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Loading failed: {ex}");

                return new LoadResult()
                {
                    Success = false,
                    StatusMessage = "Could not read the save file. The data structure is invalid."
                };
            }

            if (loadedSave == null)
            {
                return new LoadResult()
                {
                    Success = false,
                    StatusMessage = "The save file could not be parsed. It may have an unsupported version or an invalid data structure."
                };
            }

            LoadedSaveFile = loadedSave;
            SaveFileChanged?.Invoke(loadedSave);
            Debug.WriteLine(LoadedSaveFile.ToJson());

            CurrentFilePath = filePath;
            FilePathChanged?.Invoke(CurrentFilePath);

            CurrentSaveFileVersion = version;
            SaveFileVersionChanged?.Invoke(CurrentSaveFileVersion);

            return new LoadResult()
            {
                Success = true,
                Version = version,
                StatusMessage = "Save file successfully loaded."
            };
        }

        /// <summary>
        /// Saves the currently loaded save file to the current file path asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task completes when the save operation finishes.</returns>
        public async Task<SaveResult> Save()
        {
            if (LoadedSaveFile == null
                || string.IsNullOrWhiteSpace(CurrentFilePath)
                || string.IsNullOrWhiteSpace(CurrentSaveFileVersion))
            {
                return new SaveResult()
                {
                    Success = false,
                    StatusMessage = "No save file loaded. Saving is not possible."
                };
            }

            try
            {
                string json = LoadedSaveFile.ToJson();
                await File.WriteAllTextAsync(CurrentFilePath, json);

                return new SaveResult()
                {
                    Success = true,
                    StatusMessage = "Save file successfully saved."
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Saveing failed: {ex}");

                return new SaveResult()
                {
                    Success = false,
                    StatusMessage = "Unable to update the save file."
                };
            }
        }

        /// <summary>
        /// Resets the current save file state, clearing all loaded data and file information.
        /// </summary>
        public void Reset()
        {
            if (LoadedSaveFile == null
                && string.IsNullOrWhiteSpace(CurrentFilePath)
                && string.IsNullOrWhiteSpace(CurrentSaveFileVersion)) return;

            LoadedSaveFile = null;
            CurrentFilePath = string.Empty;
            CurrentSaveFileVersion = string.Empty;

            SaveFileChanged?.Invoke(LoadedSaveFile);
            FilePathChanged?.Invoke(CurrentFilePath);
            SaveFileVersionChanged?.Invoke(CurrentSaveFileVersion);
        }
    }

    #region result container
    public sealed class LoadResult
    {
        public bool Success { get; init; }
        public string? Version { get; init; }
        public string StatusMessage { get; init; } = string.Empty;
    }

    public sealed class SaveResult
    {
        public bool Success { get; init; }
        public string StatusMessage { get; init; } = string.Empty;
    }
    #endregion
}
