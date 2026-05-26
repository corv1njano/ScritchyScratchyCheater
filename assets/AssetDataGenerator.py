from pathlib import Path
from PIL import Image

SPRITE_SIZE = 100
BACKGROUND_COLOR = (0, 0, 0, 0)

OUTPUT_IMAGE_NAME = "SpriteImages.png"
OUTPUT_JSON_NAME = "SpriteData.json"
GAME_VERSION = "1.1.19d"

EXCLUDED_FOLDER_NAMES = ["upgrades"]


def next_power_of_two_grid(count: int) -> int:
    if count <= 0:
        return 1
    grid = 1
    while grid * grid < count:
        grid *= 2
    return grid


def to_pascal_case(text: str) -> str:
    if not text:
        return "Unknown"
    normalized = text.replace("-", "_").replace(" ", "_")
    parts = [part for part in normalized.split("_") if part]
    if not parts:
        return "Unknown"
    return "".join(part[:1].upper() + part[1:] for part in parts)


def to_camel_case(text: str) -> str:
    pascal = to_pascal_case(text)
    if not pascal:
        return "unknown"
    return pascal[:1].lower() + pascal[1:]


def should_exclude_folder(folder_name: str, excluded_names: list[str]) -> bool:
    return folder_name in excluded_names


def fit_image_into_square(
    img: Image.Image,
    square_size: int,
    background_color=(0, 0, 0, 0)
) -> Image.Image:
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
    with open(file_path, "w", encoding="utf-8") as f:
        f.write("{\n")
        f.write(f'    "latestGameVersion": "{data["gameVersion"]}",\n')
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
    print(f"Working directory: {script_dir}")

    all_subfolders = [f for f in script_dir.iterdir() if f.is_dir()]
    excluded = [f.name for f in all_subfolders if should_exclude_folder(f.name, EXCLUDED_FOLDER_NAMES)]
    subfolders = [f for f in all_subfolders if not should_exclude_folder(f.name, EXCLUDED_FOLDER_NAMES)]

    if excluded:
        print(f"Excluded folders: {', '.join(excluded)}")
    print(f"\nFound {len(subfolders)} folder(s) to process: {', '.join(f.name for f in subfolders)}")

    collected_sprites = []
    skipped = 0

    for folder in sorted(subfolders, key=lambda p: p.name.lower()):
        png_files = sorted([
            file for file in folder.iterdir()
            if file.is_file() and file.suffix.lower() == ".png"
        ])

        if not png_files:
            print(f"  [{folder.name}] No PNG files found, skipping.")
            continue

        sprite_type = to_camel_case(folder.name)
        print(f"  [{folder.name}] Processing {len(png_files)} sprite(s) as type '{sprite_type}' ...")

        for file_path in png_files:
            try:
                with Image.open(file_path) as img:
                    prepared = fit_image_into_square(img, SPRITE_SIZE, BACKGROUND_COLOR)

                collected_sprites.append({
                    "type": sprite_type,
                    "name": to_camel_case(file_path.stem),
                    "image": prepared
                })

            except Exception as e:
                print(f"    WARNING: Failed to load '{file_path.name}': {e}")
                skipped += 1
                continue

    total_sprites = len(collected_sprites)
    print(f"\nCollected {total_sprites} sprite(s) total ({skipped} skipped due to errors).")

    if total_sprites == 0:
        print("No sprites to process. Exiting.")
        return

    grid_size = next_power_of_two_grid(total_sprites)
    sheet_width = grid_size * SPRITE_SIZE
    sheet_height = grid_size * SPRITE_SIZE
    print(f"Sprite sheet layout: {grid_size}x{grid_size} grid -> {sheet_width}x{sheet_height}px")

    final_sheet = Image.new("RGBA", (sheet_width, sheet_height), BACKGROUND_COLOR)

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
        "gameVersion": GAME_VERSION,
        "spriteSize": SPRITE_SIZE,
        "spriteDatasets": [
            {"type": dataset_type, "data": sprite_datasets[dataset_type]}
            for dataset_type in sorted(sprite_datasets.keys())
        ]
    }

    output_image_path = script_dir / OUTPUT_IMAGE_NAME
    output_json_path = script_dir / OUTPUT_JSON_NAME

    print(f"\nSaving sprite sheet to: {output_image_path.name}")
    final_sheet.save(output_image_path)

    print(f"Saving JSON data to:    {output_json_path.name}")
    write_pretty_json(output_json_path, json_data)

    print(f"\nDone! {total_sprites} sprites across {len(sprite_datasets)} type(s): {', '.join(sorted(sprite_datasets.keys()))}")
    print("\n========================================================================\n")


if __name__ == "__main__":
    main()
    input("Press Enter to exit...")