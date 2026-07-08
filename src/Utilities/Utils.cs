using System.Diagnostics;

namespace ScritchyScratchyCheater.Utilities
{
    /// <summary>
    /// General-purpose utility methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Opens the specified URL in the default system browser.
        /// </summary>
        /// <param name="url">URL to open.</param>
        public static void OpenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        /// <summary>
        /// Creates a filter predicate for a collection view that matches items by name using a case-insensitive search.
        /// </summary>
        /// <typeparam name="T">The type of item in the collection.</typeparam>
        /// <param name="nameSelector">A function that retrieves the name from an item.</param>
        /// <param name="searchSelector">A function that retrieves the current search term.</param>
        /// <returns>A predicate that returns true if the item matches the search term, or if the search term is empty.</returns>
        public static Predicate<object> CreateNameFilter<T>(Func<T, string?> nameSelector, Func<string> searchSelector)
        {
            return item =>
            {
                if (item is not T entry) return false;
                if (string.IsNullOrWhiteSpace(searchSelector())) return true;
                return nameSelector(entry)?.Contains(searchSelector(), StringComparison.OrdinalIgnoreCase) ?? false;
            };
        }

        /// <summary>
        /// Converts a double value time counted in seconds to a pretty formatted string representation.
        /// </summary>
        /// <param name="timeInSeconds">Total time to convert in seconds.</param>
        /// <returns>Pretty time formatted as "hh, mm, ss".</returns>
        public static string GetTimeFormatted(double timeInSeconds)
        {
            if (timeInSeconds <= 0) return "0h, 0m, 0s";

            int totalSeconds = (int)timeInSeconds;

            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            return $"{hours}h, {minutes}m, {seconds}s";
        }
    }
}
