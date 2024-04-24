using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net.Http;
using System.Diagnostics;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using System.Security.Cryptography;

namespace Media_store {

    //https://learn.microsoft.com/en-us/dotnet/api/system.xml.linq.xdocument?view=net-8.0  XDocument instead of XmlDocument since it seems way better...
    public static class APIHandler {
        private static string URL = "https://hex.cse.kau.se/~jonavest/csharp-api/";

        public static async Task<string> Refresh() {
            try {
                bool hasUpdated = false;
                var file = CSVHandler._csvFile;

                using (HttpClient client = new HttpClient()) {
                    HttpResponseMessage response = await client.GetAsync(URL);
                    if (response.IsSuccessStatusCode) {

                        string content = await response.Content.ReadAsStringAsync();

                        XDocument APIdocument = XDocument.Parse(content);

                        List<Item> localCSVitems = await CSVHandler.ReadDataFromCSVAsync();

                        var lastseed = APIdocument.Root.Element("metadata").Element("lastseed")?.Value;
                        Debug.WriteLine($"lastseed: {lastseed}");

                        foreach (XElement product in APIdocument.Root.Element("products").Elements()) {
                            
                            if (await CheckForUpdate(localCSVitems, product)) {
                                hasUpdated = true;
                            }
                        }
                        if (hasUpdated) {
                            await CSVHandler.SaveItemsToCSV(localCSVitems);

                            ContentDialog infoDialog = new ContentDialog() {
                                Title = "Update!",
                                Content = $"We've updated your inventory to match the API.",
                                PrimaryButtonText = "Ok"
                            };
                            await infoDialog.ShowAsync();
                        }

                        return content;
                    } else {
                        Console.WriteLine($"API REQUEST failed with status code: {response.StatusCode}");
                        ContentDialog errorDialog = new ContentDialog() {
                            Title = "Error",
                            Content = response.StatusCode,
                            PrimaryButtonText = "OK"
                        };
                        await errorDialog.ShowAsync();
                        return null;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while making API request. msg: {ex.Message}");
                return null;
            }
        }

        // Version 2 code below
        private static async Task<bool> UpdateAPI(int PID, int stock) {
            try {
                string updateURL = $"{URL}?action=update&id={PID}&stock={stock}";

                using (HttpClient client = new HttpClient()) {
                    HttpResponseMessage response = await client.GetAsync(updateURL);
                    if(response.IsSuccessStatusCode) {
                        Debug.WriteLine($"API update successful for product ID: {PID}, new stock: {stock}");
                    } else {
                        Debug.WriteLine($"API update failed with status code: {response.StatusCode}");
                    }
                }
                return true;
            }catch (Exception ex) {
                Debug.WriteLine($"An error occurred while updating API. msg: {ex.Message}");
                return false;
            }
        }

        private static async Task<bool> CheckForUpdate(List<Item> items, XElement product) {
            bool updated = false;
            int PID = int.Parse(product.Element("id").Value);
            int API_stock = int.Parse(product.Element("stock").Value);
            double API_price = double.Parse(product.Element("price").Value);

            foreach (Item local_item in items) {
                if (local_item.PID == PID) {

                    if (local_item.Price != API_price) {
                        local_item.UpdatePrice(API_price);
                        updated = true;
                    }
                    // Version 2: Update the API if we've sold items.
                    if (local_item.Stock != API_stock) {

                        if (local_item.Stock < API_stock) {
                            await UpdateAPI(PID, local_item.Stock);
                        }else {
                            local_item.UpdateStock(API_stock);
                            updated = true;
                        }
                    }
                }
            }
            return updated;
        }
    }
}
