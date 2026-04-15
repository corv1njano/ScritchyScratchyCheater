using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ScritchyScratchyCheater.Services
{
    public class ResourceParser
    {
        public IReadOnlyDictionary<string, BitmapSource> Sprites => _sprites;

        private readonly Dictionary<string, BitmapSource> _sprites = new();

        private const string JsonResourcePath = "pack://application:,,,/Resources/Data/sprite.json";
        private const string SpriteSheetResourcePath = "pack://application:,,,/Resources/Images/sprite_images.png";

        public async Task<IReadOnlyDictionary<string, BitmapSource>> LoadSpritesAsync()
        {
            var sprites = await Task.Run(() =>
            {
                var root = LoadSpriteData(JsonResourcePath);
                var spriteSheet = LoadSpriteSheet(SpriteSheetResourcePath);

                if (root == null) throw new ArgumentNullException("sprite.json could not be loaded or parsed.");
                if (root.SpriteDatasets == null || root.SpriteDatasets.Count == 0) throw new InvalidOperationException("sprite.json does not contain any data sets.");
                if (root.SpriteSize <= 0) throw new InvalidOperationException("Sprite size must be bigger than 0 pixels.");

                var result = new Dictionary<string, BitmapSource>(StringComparer.Ordinal);
                var entries = root.SpriteDatasets
                    .Where(dataset => dataset.Data != null)
                    .SelectMany(dataset => dataset.Data!);

                foreach (var entry in entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.Name)) continue;
                    if (result.ContainsKey(entry.Name)) throw new InvalidOperationException($"Duplicate found: '{entry.Name}'");

                    var rect = new Int32Rect(
                        entry.X * root.SpriteSize,
                        entry.Y * root.SpriteSize,
                        root.SpriteSize,
                        root.SpriteSize);
                    var croppedBitmap = new CroppedBitmap(spriteSheet, rect);
                    croppedBitmap.Freeze();
                    result.Add(entry.Name, croppedBitmap);
                }

                return result;
            });

            _sprites.Clear();

            foreach (var pair in sprites) _sprites[pair.Key] = pair.Value;

            return Sprites;
        }

        /// <summary>
        /// Get the sprite image associated with the specified name.
        /// </summary>
        /// <remarks>If the specified sprite name does not exist in the internal collection or is invalid,
        /// the method returns <see langword="null"/>. The search is case-sensitive.</remarks>
        /// <param name="spriteName">The name of the sprite to get.</param>
        /// <returns>The requested sprite as a <see cref="BitmapSource"/> if found; otherwise, <see langword="null"/>.</returns>
        public BitmapSource? GetSprite(string spriteName)
        {
            if (string.IsNullOrWhiteSpace(spriteName)) return null;

            return _sprites.TryGetValue(spriteName, out var sprite) ? sprite : null;
        }

        /// <summary>
        /// Checks if a sprite with the given name exists in the collection.
        /// </summary>
        /// <remarks>
        /// The name check is case-sensitive. Use this method before trying to access a sprite by name.
        /// </remarks>
        /// <param name="spriteName">The name of the sprite to look for. Must not be null, empty, or only white space.</param>
        /// <returns>true if a sprite with this name exists; otherwise, false.</returns>
        public bool HasSprite(string spriteName)
        {
            return !string.IsNullOrWhiteSpace(spriteName) && _sprites.ContainsKey(spriteName);
        }

        private static SpriteRoot? LoadSpriteData(string uriSource)
        {
            var uri = new Uri(uriSource, UriKind.Absolute);
            var resourceInfo = Application.GetResourceStream(uri);

            if (resourceInfo?.Stream == null) return null;

            using var reader = new StreamReader(resourceInfo.Stream);
            var json = reader.ReadToEnd();

            return JsonSerializer.Deserialize<SpriteRoot>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private static BitmapImage LoadSpriteSheet(string uriSource)
        {
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(uriSource, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }
    }

    public sealed class SpriteRoot
    {
        public string LatestVersion { get; set; } = string.Empty;
        public int SpriteSize { get; set; }
        public List<SpriteDataset>? SpriteDatasets { get; set; }
    }

    public sealed class SpriteDataset
    {
        public string Type { get; set; } = string.Empty;
        public List<SpriteEntry>? Data { get; set; }
    }

    public sealed class SpriteEntry
    {
        public string Name { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
    }
}
