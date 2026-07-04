namespace ScritchyScratchyCheater.Models.GameData
{
    public sealed class Loan
    {
        /// <summary>
        /// ID is the actual name of the item iteself in-game, so no need for a Name-property.
        /// </summary>
        public string Id { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        /// <summary>
        /// Servity value levels. [0] is servity 1, [1] is servity 2 and so on.
        /// </summary>
        public int[] Levels { get; init; } = new int[3];
    }
}
