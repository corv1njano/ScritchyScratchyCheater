namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Upgrade : GameDataEntry
    {
        public int MaxBuyCount { get; init; }
    }

    public enum UpgradeType
    {
        Normal,
        Gadget
    }
}
