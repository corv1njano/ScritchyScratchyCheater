using System.Text.Json.Serialization;

namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Cosmetic : GameDataEntry
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CosmeticCategory Category { get; set; } = CosmeticCategory.Other;
    }

    public sealed class CosmeticCategoryItem
    {
        public CosmeticCategory Category { get; init; }
        public string Name { get; init; } = string.Empty;

        public override string ToString() { return Name; }
    }

    public enum CosmeticCategory
    {
        Other,
        Phone,
        Table,
        TrashCan,
        ElectricFan,
        Mundo,
        EggTimer,
        ScratchBot
    }
}
