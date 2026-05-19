namespace ScritchyScratchyCheater.Models.Results
{
    public sealed class LoadResult
    {
        public bool Success { get; init; }
        public string? Version { get; init; }
        public string StatusMessage { get; init; } = string.Empty;
    }
}
