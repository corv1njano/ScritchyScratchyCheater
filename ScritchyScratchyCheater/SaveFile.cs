using System.IO;
using System.Text.Json;

namespace ScritchyScratchyCheater
{
    internal class SaveFile
    {
        public string SaveVersion { get; set; } = string.Empty;
        public int PrestigeCurrency { get; set; }
        public double Tokens { get; set; }
        public decimal Money { get; set; }

        public SaveFile()
        {
        }

        public void Load(string path)
        {
            string json = File.ReadAllText(path);
            using JsonDocument doc = JsonDocument.Parse(json);

            var root = doc.RootElement;

            SaveVersion = root.GetProperty("saveVersion").GetString() ?? string.Empty;
            PrestigeCurrency = root.GetProperty("prestigeCurrency").GetInt32();
            Tokens = root.GetProperty("tokens").GetDouble();
            Money = FindDecimal(root, "money") ?? 0m;
        }

        /// <summary>
        /// money is stored under root > layerOne > money
        /// </summary>
        private decimal? FindDecimal(JsonElement element, string propertyName)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in element.EnumerateObject())
                {
                    if (prop.NameEquals(propertyName)) return prop.Value.GetDecimal();

                    var found = FindDecimal(prop.Value, propertyName);
                    if (found.HasValue) return found;
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    var found = FindDecimal(item, propertyName);
                    if (found.HasValue) return found;
                }
            }

            return null;
        }
    }
}