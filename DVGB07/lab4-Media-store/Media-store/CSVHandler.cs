using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Media_store;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Windows.Devices.Geolocation;
using Windows.Devices.Printers;
using System.Collections.ObjectModel;
using System.Text;
using Windows.System;
using System.Xml.Linq;
using Windows.Storage.Pickers;


namespace Media_store {

    public static class CSVHandler {

        public static StorageFile _csvFile;
        private const string ASCII_HEADER = "    __  __          _ _             _                 \r\n   |  \\/  | ___  __| (_) __ _   ___| |_ ___  _ __ ___ \r\n   | |\\/| |/ _ \\/ _` | |/ _` | / __| __/ _ \\| '__/ _ \\\r\n   | |  | |  __/ (_| | | (_| | \\__ \\ || (_) | | |  __/\r\n   |_|  |_|\\___|\\__,_|_|\\__,_| |___/\\__\\___/|_|  \\___|";
        private static string DATE = DateTime.Now.ToShortDateString();
        private static string TIME = DateTime.Now.ToString("HH:mm:ss");

        public async static Task<bool> OpenCsvFileAsync(){
            var picker = new FileOpenPicker{
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".csv");

            _csvFile = await picker.PickSingleFileAsync();

            return _csvFile != null;
        }

        public async static Task<string>  ReadTextStringFromDB() {
            try {
                if (_csvFile != null){
                    var result = await FileIO.ReadTextAsync(_csvFile);
                    return result;
                }else{
                    Debug.WriteLine("No file!!!!!!!!!!!!!!");
                    return null;
                }

            }catch (Exception ex) {
                Debug.WriteLine($"Error from ReadTextStringFromDB() ex: {ex.Message}");
                return null;
            }
        }

        public static async Task<List<Item>> ReadDataFromCSVAsync() {
            List<Item> items = new List<Item>();

            try {
                string localFileText = await ReadTextStringFromDB();

                using (StringReader reader = new StringReader(localFileText)) {

                    string line = reader.ReadLine(); // skip first line

                    while ((line = await reader.ReadLineAsync()) != null) {
                        string[] field = line.Split(',');

                        int PID = int.Parse(field[1]);
                        string name = field[2];
                        int price = int.Parse(field[3]);

                        int stock = int.Parse(field[4]);

                        if (field[0].Contains("Book")) {
                            string author = field.Length > 5 ? field[5] : string.Empty;
                            string genre = field.Length > 6 ? field[6] : string.Empty;
                            string format = field.Length > 7 ? field[7] : string.Empty;
                            string language = field.Length > 8 ? field[8] : string.Empty;

                            items.Add(new Book(PID, name, price, stock, author, genre, format, language));

                        } else if (field[0].Contains("Game")) {
                            string platform = field.Length > 9 ? field[9] : string.Empty;
                            items.Add(new Game(PID, name, price, stock, platform));

                        } else if (field[0].Contains("Movie")) {
                            string format = field.Length > 7 ? field[7] : string.Empty;
                            int length;

                            if (field.Length > 10 && int.TryParse(field[10], out length)) {
                                items.Add(new Movie(PID, name, price, stock, format, length));
                            } else {
                                items.Add(new Movie(PID, name, price, stock, format));
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while reading data from CSV: {ex.Message}");
            }
            return items;
        }
        public static async void AddDataToCSVAsync(Object item, int amount) {
            try {

                var localFile = _csvFile;
                
                var localFileText = await ReadTextStringFromDB();

                string newItem;
                if (item is Book book) {
                    newItem = $"{book.Type},{book.PID},{book.Name},{book.Price},{amount},{book.Author},{book.Genre},{book.Format},{book.Language},,,,";
                } else if (item is Game game) {
                    newItem = $"{game.Type},{game.PID},{game.Name},{game.Price},{amount},,,,,{game.Platform},";
                } else if (item is Movie movie) {
                    newItem = $"{movie.Type},{movie.PID},{movie.Name},{movie.Price},{amount},,,{movie.Format},,,{movie.Length}";
                } else {
                    throw new ArgumentException("Unsupported item type.");
                }

                using (StringReader reader = new StringReader(localFileText)) {

                    string line = reader.ReadToEnd(); //GOTO end

                    string newLine = line + Environment.NewLine + newItem; // Environment.NewLine == "\r\n". but Environment.NewLine is platform independant.
                    await FileIO.WriteTextAsync(localFile, newLine);
                }

            } catch (Exception ex) {
                Debug.WriteLine($"An error occurred while writing data to CSV: {ex.Message}");
            }
        }
        public static async Task<int> CreateUniquePIDAsync() {
            List<int> exisitingPIDS = await GetExistingPIDSAsync();

            int newPID = 1;

            while (exisitingPIDS.Contains(newPID)) {
                newPID++;
            }

            return newPID;
        }

        private static async Task<List<int>> GetExistingPIDSAsync() {
            List<int> existingPIDS = new List<int>();

            try {
                var localFileText = await ReadTextStringFromDB();

                using (StringReader reader = new StringReader(localFileText)) {

                    string line = reader.ReadLine(); // skip first line

                    while ((line = await reader.ReadLineAsync()) != null) {
                        string[] field = line.Split(',');
                        existingPIDS.Add(int.Parse(field[1])); // Field[1] == PID. Hardcoded. Might fix, should fix.
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine($"{ex.Message}");
            }
            return existingPIDS;
        }

        public async static Task<string> UpdateAmountOfXByPID(int PIDToUpdate, int amountToUpdate, bool isAddition) {
            try {
                var localFile = _csvFile;

                var localFileText = await ReadTextStringFromDB();

                List<string> CSVList = new List<string>();
                string resultMessage = "updateAmount-placeholdertext";

                using (StringReader reader = new StringReader(localFileText)) {
                    string line = reader.ReadLine(); // skip first line
                    CSVList.Add(line); // add first line back

                    while ((line = await reader.ReadLineAsync()) != null) {
                        string[] field = line.Split(',');

                        int currStock = int.Parse(field[4]);

                        if (int.TryParse(field[1], out int dbPID)) {
                            if (dbPID == PIDToUpdate) {
                                int newStock = isAddition ? currStock + amountToUpdate : currStock - amountToUpdate;

                                if (!isAddition && newStock < 0) {
                                    return $"Trying to delete too many items. Current stock is {currStock}";
                                }

                                if (newStock == 0) {
                                    resultMessage = isAddition ? $"Added the whole inventory of: {currStock} items" : $"Deleted the whole inventory of: {currStock} items";
                                } else {
                                    resultMessage = isAddition ? $"Added {amountToUpdate} items." : $"Deleted {amountToUpdate} items.";
                                }

                                field[4] = newStock.ToString();
                                string updatedLine = string.Join(",", field);
                                CSVList.Add(updatedLine);
                                continue;
                            }
                        }

                        CSVList.Add(line);
                    }
                }

                string csvText = string.Join(Environment.NewLine, CSVList); // because WriteTextAsync expects a single string..
                await FileIO.WriteTextAsync(localFile, csvText);
                return resultMessage;
            } catch (Exception ex) {
                Debug.WriteLine($"UpdateAmountOfXByPID Error: {ex}");
                return null;
            }
        }
        public async static Task<string> DeleteItemByPID(int PID2Delete) {
            try {

                var localFile = _csvFile;

                var localFileText = await FileIO.ReadTextAsync(localFile);
        
                List<string> CSVList = new List<string>();
        
                using (StringReader reader = new StringReader(localFileText)) {
                    string line = reader.ReadLine(); // skip first line
                    CSVList.Add(line); // add first line back
        
                    while ((line = await reader.ReadLineAsync()) != null) {
                        string[] field = line.Split(',');
        
                        int dbPID = int.Parse(field[1]);

                        if (dbPID == PID2Delete) {
                            Debug.WriteLine($"Skipping: {field[2]}");
                            continue; //Skip line aka remove it
                        }
                        
                        CSVList.Add(line);
                    }
        
                }
        
                string csvText = string.Join(Environment.NewLine, CSVList);
                await FileIO.WriteTextAsync(localFile, csvText);
                return "Item deleted successfully.";
        
            } catch (Exception ex) {
                Debug.WriteLine($"DeleteItemByPID Error: {ex}");
                return null;
            }
        }
        public async static Task<string> DeleteAmountOfXByPID(int PID2Delete, int? amount2Delete) {
            if (amount2Delete == null) {
                return "Amount to delete cannot be null.";
            }

            return await UpdateAmountOfXByPID(PID2Delete, amount2Delete.Value, false);
        }
        public async static Task<string> AddAmountOfXByPID(int PID2Add, int amount2Add) {
            return await UpdateAmountOfXByPID(PID2Add, amount2Add, true);
        }


        //Receipt handling
        public static async void Checkout(ObservableCollection<BasketItem> basket) {
            string filename = $"Order#{DateTime.Now:yyMMddHHmmss}.txt";
            string receipt = GenerateReceipt(basket, filename);

            await SaveReceipt(receipt, filename);

            try {
                await OpenReceipt(filename);
            }catch (Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        private static async Task SaveReceipt(string content, string FileName) {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile receiptFile = await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            
            await FileIO.WriteTextAsync(receiptFile, content, Windows.Storage.Streams.UnicodeEncoding.Utf8);
        }
        private static string GenerateReceipt(ObservableCollection<BasketItem> basket, string filename) {
            StringBuilder receipt = new StringBuilder();

            int qtyWidth = 5;
            int nameWidth = 30;
            int priceWidth = 10;
            double sum = 0;
            DATE = DateTime.Now.ToShortDateString();
            TIME = DateTime.Now.ToString("HH:mm:ss");

            receipt.AppendLine(ASCII_HEADER);
            receipt.AppendLine($"\t\t\t\t{DATE} {TIME}\n");
            receipt.AppendLine($"{filename.Split('.')[0]}");
            receipt.AppendLine(new string('-', qtyWidth + nameWidth + priceWidth));

            foreach (var item in basket) {
                string qty = $"[x{item.Quantity}]".PadRight(qtyWidth);
                string name = item.ItemName.PadRight(nameWidth);
                string price = $"{item.Item.Price}:-".PadRight(priceWidth);
                receipt.AppendLine($"{qty}{name}{price}");
                sum += (item.Item.Price * item.Quantity);
            }
            receipt.AppendLine(new string('-', qtyWidth + nameWidth + priceWidth));
            string totalLine = $"Order total: {sum}:-";
            receipt.AppendLine(totalLine);

            return receipt.ToString();
        }
        private static async Task OpenReceipt(string filename) {
            try {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.GetFileAsync(filename);

                var options = new LauncherOptions(); 
                options.DisplayApplicationPicker = true;

                await Launcher.LaunchFileAsync(file);

            } catch (FileNotFoundException) {
                Debug.WriteLine("file not found.");
            } catch (Exception ex) {
                Debug.WriteLine($"Error opening file: {ex.Message}");
            }
        }

        //Laboration 5:
        public static async Task SaveItemsToCSV(List<Item> items) {
            try {
                var localFile = _csvFile;
                StringBuilder csvContent = new StringBuilder();

                csvContent.AppendLine("Type,PID,Name,Price,Stock,Author,Genre,Format,Language,Platform,Length");

                foreach (var item in items) {
                    if (item is Book book) {
                        csvContent.AppendLine($"{book.Type},{book.PID},{book.Name},{book.Price},{book.Stock},{book.Author},{book.Genre},{book.Format},{book.Language},,");
                    } else if (item is Game game) {
                        csvContent.AppendLine($"{game.Type},{game.PID},{game.Name},{game.Price},{game.Stock},,,,,{game.Platform}");
                    } else if (item is Movie movie) {
                        csvContent.AppendLine($"{movie.Type},{movie.PID},{movie.Name},{movie.Price},{movie.Stock},,,{movie.Format},,,{movie.Length}");
                    }
                }

                await FileIO.WriteTextAsync(localFile, csvContent.ToString());

            } catch (Exception ex) {
                Debug.WriteLine($"An error occurred while writing data to CSV: {ex.Message}");
            }
        }

    }
}

 