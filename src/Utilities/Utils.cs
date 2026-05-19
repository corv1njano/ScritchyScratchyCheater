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
            if (string.IsNullOrEmpty(url)) return;

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
