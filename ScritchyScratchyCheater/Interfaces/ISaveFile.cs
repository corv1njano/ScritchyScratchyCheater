namespace ScritchyScratchyCheater.Interfaces
{
    public interface ISaveFile
    {
        /// <summary>
        /// Stores the save file version as a string under root > saveVersion.
        /// Is needed to check the save version and load the correct save editor form.
        /// </summary>
        string? SaveVersion { get; }

        /// <summary>
        /// Converts the save file into a JSON structure.
        /// </summary>
        /// <returns>The save file object as a JSON string.</returns>
        string ToJson();
    }
}
