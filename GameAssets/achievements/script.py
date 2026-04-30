import os

# Verzeichnis, in dem das Skript liegt
script_dir = os.path.dirname(os.path.abspath(__file__))

for filename in os.listdir(script_dir):
    if filename.lower().endswith(".png") and not filename.startswith("a_"):
        old_path = os.path.join(script_dir, filename)
        new_name = "a_" + filename
        new_path = os.path.join(script_dir, new_name)

        os.rename(old_path, new_path)
        print(f"Renamed: {filename} -> {new_name}")