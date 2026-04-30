from pathlib import Path
from PIL import Image

SPRITE_SIZE = 100
BACKGROUND_COLOR = (0, 0, 0, 0)

OUTPUT_IMAGE_NAME = "sprite_images.png"
OUTPUT_JSON_NAME = "sprite_data.json"
GAME_VERSION = "1.0.30d"

EXCLUDED_FOLDER_NAMES = ["upgrades"]


def next_power_of_two_grid(count: int) -> int:
    """
    Returns the number of cells per axis using powers of two:
    1x1, 2x2, 4x4, 8x8, 16x16, ...
    """
    if count <= 0:
        return 1

    grid = 1
    while grid * grid < count:
        grid *= 2
    return grid


def to_pascal_case(text: str) -> str:
    """
    Converts text like 'item_icons' or 'item-icons' to 'ItemIcons'.
    """
    if not text:
        return "Unknown"

    normalized = text.replace("-", "_").replace(" ", "_")
    parts = [part for part in normalized.split("_") if part]

    if not parts:
        return "Unknown"

    return "".join(part[:1].upper() + part[1:] for part in parts)


def to_camel_case(text: str) -> str:
    """
    Converts text like 'item_icons' to 'itemIcons'.
    """
    pascal = to_pascal_case(text)
    if not pascal:
        return "unknown"

    return pascal[:1].lower() + pascal[1:]


def should_exclude_folder(folder_name: str, excluded_names: list[str]) -> bool:
    """
    Returns True if the folder name exactly matches any excluded name.
    """
    return folder_name in excluded_names


def fit_image_into_square(
    img: Image.Image,
    square_size: int,
    background_color=(0, 0, 0, 0)
) -> Image.Image:
    """
    Scales an image proportionally into a square and centers it.
    Uses NEAREST to preserve pixel-art sharpness.
    """
    img = img.convert("RGBA")
    original_width, original_height = img.size

    scale = min(square_size / original_width, square_size / original_height)

    new_width = max(1, round(original_width * scale))
    new_height = max(1, round(original_height * scale))

    resized = img.resize((new_width, new_height), Image.Resampling.NEAREST)

    square = Image.new("RGBA", (square_size, square_size), background_color)

    x = (square_size - new_width) // 2
    y = (square_size - new_height) // 2

    square.paste(resized, (x, y), resized)
    return square


def write_pretty_json(file_path: Path, data: dict) -> None:
    """
    Writes the JSON file with one-line sprite entries inside each data array.
    """
    with open(file_path, "w", encoding="utf-8") as f:
        f.write("{\n")
        f.write(f'    "latestGameVersion": "{data["latestGameVersion"]}",\n')
        f.write(f'    "spriteSize": {data["spriteSize"]},\n')
        f.write('    "spriteDatasets": [\n')

        datasets = data["spriteDatasets"]

        for dataset_index, dataset in enumerate(datasets):
            f.write("        {\n")
            f.write(f'            "type": "{dataset["type"]}",\n')
            f.write('            "data": [\n')

            entries = dataset["data"]

            for entry_index, entry in enumerate(entries):
                line = (
                    f'                {{ "name": "{entry["name"]}", '
                    f'"x": {entry["x"]}, "y": {entry["y"]} }}'
                )

                if entry_index < len(entries) - 1:
                    line += ","

                f.write(line + "\n")

            f.write("            ]\n")

            if dataset_index < len(datasets) - 1:
                f.write("        },\n")
            else:
                f.write("        }\n")

        f.write("    ]\n")
        f.write("}\n")


def main() -> None:
    script_dir = Path(__file__).resolve().parent

    subfolders = [
        folder for folder in script_dir.iterdir()
        if folder.is_dir() and not should_exclude_folder(folder.name, EXCLUDED_FOLDER_NAMES)
    ]

    collected_sprites = []

    for folder in sorted(subfolders, key=lambda p: p.name.lower()):
        png_files = sorted([
            file for file in folder.iterdir()
            if file.is_file() and file.suffix.lower() == ".png"
        ])

        if not png_files:
            continue

        sprite_type = to_camel_case(folder.name)

        for file_path in png_files:
            try:
                with Image.open(file_path) as img:
                    prepared = fit_image_into_square(
                        img,
                        SPRITE_SIZE,
                        BACKGROUND_COLOR
                    )

                collected_sprites.append({
                    "type": sprite_type,
                    "name": to_camel_case(file_path.stem),
                    "image": prepared
                })

            except Exception:
                continue

    total_sprites = len(collected_sprites)

    if total_sprites == 0:
        return

    grid_size = next_power_of_two_grid(total_sprites)

    sheet_width = grid_size * SPRITE_SIZE
    sheet_height = grid_size * SPRITE_SIZE

    final_sheet = Image.new(
        "RGBA",
        (sheet_width, sheet_height),
        BACKGROUND_COLOR
    )

    sprite_datasets: dict[str, list[dict]] = {}

    for index, entry in enumerate(collected_sprites):
        row = index // grid_size
        col = index % grid_size

        pixel_x = col * SPRITE_SIZE
        pixel_y = row * SPRITE_SIZE

        final_sheet.paste(entry["image"], (pixel_x, pixel_y), entry["image"])

        entry_type = entry["type"]
        if entry_type not in sprite_datasets:
            sprite_datasets[entry_type] = []

        sprite_datasets[entry_type].append({
            "name": entry["name"],
            "x": col,
            "y": row
        })

    json_data = {
        "latestGameVersion": GAME_VERSION,
        "spriteSize": SPRITE_SIZE,
        "spriteDatasets": [
            {
                "type": dataset_type,
                "data": sprite_datasets[dataset_type]
            }
            for dataset_type in sorted(sprite_datasets.keys())
        ]
    }

    output_image_path = script_dir / OUTPUT_IMAGE_NAME
    output_json_path = script_dir / OUTPUT_JSON_NAME

    final_sheet.save(output_image_path)
    write_pretty_json(output_json_path, json_data)


if __name__ == "__main__":
    main()