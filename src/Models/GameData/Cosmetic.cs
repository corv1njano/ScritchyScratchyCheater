using System.Text.Json.Serialization;

namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Cosmetic
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string IconId { get; init; } = string.Empty;

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
        Fan,
        Mundo,
        EggTimer,
        ScratchBot
    }
}
