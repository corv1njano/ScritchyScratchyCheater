using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;

namespace ScritchyScratchyCheater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string defaultSaveFilePath;
        private string importedSaveFilePath = string.Empty;

        private SaveFile? loadedSaveFile = null;

        private readonly string[] supportedVersions = ["0.1"];

        public MainWindow()
        {
            InitializeComponent();

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string localLow = Path.Combine(Directory.GetParent(localAppData)!.FullName, "LocalLow");

            defaultSaveFilePath = Path.Combine(
                localLow,
                "Lunch Money Games",
                "Scritchy Scratchy",
                "save.json"
            );
        }

        private void LoadSaveFile_Click(object sender, RoutedEventArgs e)
        {
            loadedSaveFile = LoadSaveFileData(defaultSaveFilePath);
            SetLoadedDataToInputFields();
        }

        private void ImportSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new OpenFileDialog
            {
                Title = "Select a Save File...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Save File (*.json)|*.json",
                Multiselect = false
            };

            if (filePicker.ShowDialog() == true)
            {
                loadedSaveFile = LoadSaveFileData(filePicker.FileName);
                SetLoadedDataToInputFields();
            }
        }

        private SaveFile? LoadSaveFileData(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show(
                        $"Save file does not exist under this path:\n{filePath}",
                        "Error when loading data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }

                var saveFile = new SaveFile();
                saveFile.Load(filePath);

                Debug.WriteLine("Version: " + saveFile.SaveVersion);
                Debug.WriteLine("Money: " + saveFile.Money);
                Debug.WriteLine("Prestige: " + saveFile.PrestigeCurrency);
                Debug.WriteLine("Tokens: " + saveFile.Tokens);

                SaveData.IsEnabled = true;

                MessageBox.Show(
                    "Save file data has been loaded succesfully.",
                    "Data loaded",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                SelectedSaveFilePath.Text = $"Selected: {filePath}";
                SelectedSaveFilePath.ToolTip = filePath;

                return saveFile;
            }
            catch (JsonException)
            {
                MessageBox.Show(
                    "Save file has invalid data format.",
                    "Error when loading data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error when loading save file:\n{ex.Message}",
                    "Error when loading data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            return null;
        }

        private void SetLoadedDataToInputFields()
        {
            if (loadedSaveFile == null) return;
            InputMoney.Text = loadedSaveFile.Money.ToString();
            InputPrestige.Text = loadedSaveFile.PrestigeCurrency.ToString();
            InputTokens.Text = loadedSaveFile.Tokens.ToString();
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            if (loadedSaveFile == null) return;

            bool isImported = !string.IsNullOrEmpty(importedSaveFilePath);
            var pathToSave = isImported ? importedSaveFilePath : defaultSaveFilePath;

            if (string.IsNullOrWhiteSpace(pathToSave) || !File.Exists(pathToSave))
            {
                MessageBox.Show(
                    $"Save file does not exist under this path:\n{pathToSave}",
                    "Error when saving data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            string moneyText = InputMoney.Text.Trim();
            string prestigeText = InputPrestige.Text.Trim();
            string tokensText = InputTokens.Text.Trim();

            if (!decimal.TryParse(moneyText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal money))
            {
                MessageBox.Show(
                    "The field 'Money' is invalid.",
                    "Error when saving data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(prestigeText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int prestigeCurrency))
            {
                MessageBox.Show(
                    "The field 'Prestige' is invalid.",
                    "Error when saving data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(tokensText, NumberStyles.Any, CultureInfo.InvariantCulture, out double tokens))
            {
                MessageBox.Show(
                    "The field 'Tokens' is invalid.",
                    "Error when saving data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                string json = File.ReadAllText(pathToSave);

                JsonNode? root = JsonNode.Parse(json);
                if (root == null)
                {
                    MessageBox.Show(
                        "The save file could not be read.",
                        "Error when saving data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                JsonNode? layerOne = root["layerOne"];
                if (layerOne == null)
                {
                    MessageBox.Show(
                        "Save file has invalid data format.",
                        "Error when saving data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                layerOne["money"] = money;
                root["prestigeCurrency"] = prestigeCurrency;
                root["tokens"] = tokens;

                string updatedJson = root.ToJsonString(new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(pathToSave, updatedJson);

                loadedSaveFile.Money = money;
                loadedSaveFile.PrestigeCurrency = prestigeCurrency;
                loadedSaveFile.Tokens = tokens;

                MessageBox.Show(
                    "Save file data has been loaded saved.",
                    "Data saved",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occured while saving:\n{ex.Message}",
                    "Error when saving data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void GetHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "- 'Load Save File...': Load save file data directly from the default path where the game stores the save file." +
                "\n- 'Import Save File...' Load save file data from any path." +
                "\n\nTool made by corv1njano. Check out my GitHub!" +
                "\n\nVersion 1.0.0 (release April 2026)",
                "Help and Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void SetAllMax_Click(object sender, RoutedEventArgs e)
        {
            InputMoney.Text = (decimal.MaxValue -1).ToString(CultureInfo.InvariantCulture);
            InputPrestige.Text = (int.MaxValue -1).ToString();
            InputTokens.Text = (double.MaxValue -1).ToString(CultureInfo.InvariantCulture);
        }
    }
}