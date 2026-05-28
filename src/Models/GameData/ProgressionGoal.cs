namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class ProgressionGoal
    {
        public int Index { get; init; }
        public double Goal { get; init; }

        public override string ToString()
        {
            var formatted = Goal >= 1e10
                ? Goal.ToString("E2")
                : Goal.ToString("N0");

            return $"Goal {Index}: {formatted}";
        }
    }
}
