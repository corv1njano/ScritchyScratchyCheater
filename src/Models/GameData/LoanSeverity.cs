namespace ScritchyScratchyCheater.Models.GameData
{
    /// <summary>
    /// May be replaced... change Name to "Level " + (int)severity
    /// </summary>
    public sealed class LoanSeverity
    {
        public string Name { get; init; } = string.Empty;
        public int Severity { get; init; }

        public override string ToString() { return Name; }
    }
}
