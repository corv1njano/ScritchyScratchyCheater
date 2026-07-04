namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class LoanServity
    {
        public string Name { get; init; } = string.Empty;
        public int Servity { get; init; }

        public override string ToString() { return Name; }
    }
}
