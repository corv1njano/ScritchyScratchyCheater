namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Upgrade : GameDataEntry
    {
        public int MaxBuyCount { get; init; }
    }

    /// <summary>
    /// Currently unused.
    /// </summary>
    public enum UpgradeType
    {
        Normal,
        Gadget
    }
}
