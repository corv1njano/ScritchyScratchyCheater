namespace ScritchyScratchyCheater.Models.GameData
{
    public abstract class GameDataEntry
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string IconId { get; init; } = string.Empty;
    }
}
