namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Gadget
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string IconId { get; init; } = string.Empty;
        public int MaxBuyCount { get; init; }
    }
}
