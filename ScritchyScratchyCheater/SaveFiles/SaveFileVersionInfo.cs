using ScritchyScratchyCheater.Interfaces;

namespace ScritchyScratchyCheater.SaveFiles
{
    internal class SaveFileVersionInfo : ISaveFile
    {
        public string SaveVersion { get; set; } = string.Empty;

        public string ToJson()
        {
            return string.IsNullOrWhiteSpace(SaveVersion) ? "<no version identified>" : $"Version: '{SaveVersion}'"; 
        }
    }
}
