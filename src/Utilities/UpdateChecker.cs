using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace ScritchyScratchyCheater.Utilities
{
    public static class UpdateChecker
    {
        private const string GTIHUB_API_URL = "https://api.github.com/repos/corv1njano/ScritchyScratchyCheater/releases/latest";
    
        /// <summary>
        /// Gets the latest release tag from GtiHub project API.
        /// </summary>
        /// <remarks>Returns null when no connection could be established or parsing the response failed.</remarks>
        /// <returns>A string with the latest release tag (e.g. "rel-v1.3.0").</returns>
        public static async Task<string?> GetLatestVersionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("ScritchyScratchyCheater");
                client.Timeout = TimeSpan.FromSeconds(5);

                var response = await client.GetAsync(GTIHUB_API_URL);
                if (!response.IsSuccessStatusCode) return null;

                var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);

                return document.RootElement.GetProperty("tag_name").GetString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update Checker failed: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Checks if the latest release version is newer than the currently running version.
        /// </summary>
        /// <param name="latestTag">The tag name of the latest release (e.g. "rel-v1.3.0").</param>
        /// <returns>True if a newer version is available, otherwise false.</returns>
        public static bool IsNewerVersion(string latestVersion)
        {
            var current = Assembly.GetExecutingAssembly().GetName().Version;
            //var current = new Version("1.2.0.0"); // test
            var latestClean = latestVersion.Replace("rel-v", string.Empty);

            return current != null 
                && Version.TryParse(latestClean, out var latest)
                && latest > current;
        }
    }
}
