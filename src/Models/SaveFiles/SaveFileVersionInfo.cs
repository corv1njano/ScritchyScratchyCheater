using System.Text.Json;

namespace ScritchyScratchyCheater.Models.SaveFiles
{
    internal class SaveFileVersionInfo : ISaveFile
    {
        public string SaveVersion { get; set; } = string.Empty;

        public string GetVersion()
        {
            return string.IsNullOrWhiteSpace(SaveVersion) ? "<no version identified>" : $"Version: '{SaveVersion}'";
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, App.JsonOptions);
        }
    }
}
