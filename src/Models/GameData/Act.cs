namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Act
    {
        public string Name { get; init; } = string.Empty;
        public int ActId { get; init; }

        public override string ToString() { return Name; }
    }
}
