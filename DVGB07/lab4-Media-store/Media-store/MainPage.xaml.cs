using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Media_store {
    public sealed partial class MainPage : Page {

        public ObservableCollection<BasketItem> BuyCollection;
        private static bool init = false; //static so that it is stored across both pages!

        public MainPage() {
            this.InitializeComponent();
            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            //this.DataContext = localFolder;

            inventoryView.ItemClicked += InventoryView_ItemClicked;

            BuyCollection = new ObservableCollection<BasketItem>();
            BuyList.ItemsSource = BuyCollection;

        }
        //OpenFilepicker only once.... fkn uwp
        protected override void OnNavigatedTo(NavigationEventArgs e){
            base.OnNavigatedTo(e);
            if (!init){
                FetchFile();
            }
        }
        private async void FetchFile(){
            try{
                if(await CSVHandler.OpenCsvFileAsync()){
                    Debug.WriteLine("OpenCsvFileAsync IS TRUEEEEEEEEEEE");
                    inventoryView.LoadData();
                    init = true;
                }else{
                    Debug.WriteLine("OpenCsvFileAsync aint!!!! TRUEEEEEEEEEEE");
                }
                Debug.WriteLine($"init: {init}");
            }
            catch (Exception ex){
                Debug.WriteLine(ex);
            }
        }
        private async void InventoryView_ItemClicked(object sender, InventoryItemClickEventArgs e) {
            Item clickedItem = e.ClickedItem;

            try {

                if (clickedItem.Stock > 0) {
                    BasketItem basketItem = null;

                    
                    foreach (BasketItem tmpItem in BuyCollection) {
                        if (tmpItem.ItemName == clickedItem.Name) {
                            basketItem = tmpItem;
                        }
                    }

                    // If item clicked on doesn't exists in the basket, create a new basketItem and adding to BuyCollection.
                    if (basketItem == null) {
                        Debug.WriteLine($"Item doesnt exist in basket, adding new one: {clickedItem.Name}");
                        basketItem = new BasketItem {
                            Item = clickedItem,
                            Quantity = 1
                        };

                        basketItem.PropertyChanged += BasketItem_PropertyChanged;
                        BuyCollection.Add(basketItem);
                    } else {
                        basketItem.Quantity++;
                        Debug.WriteLine($"Item already exist in basket,increasing quantity: {basketItem.Quantity}");
                    }

                    //Decrement in CSV file
                    await CSVHandler.DeleteAmountOfXByPID(clickedItem.PID, 1);
                    inventoryView.LoadData();

                } else {
                    ShowOutOfStockMessage(clickedItem.Name);
                }

            } catch (Exception ex) {
                Debug.WriteLine($"InvView_ItemClicked from MainPage, ERROR: {ex}");
            }

        }
        private void BasketItem_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "Quantity") {
                BuyList.ItemsSource = null;
                BuyList.ItemsSource = BuyCollection;
            }
        }

        private async void BasketItem_Tapped(object sender, TappedRoutedEventArgs e) {
            if (sender is FrameworkElement element && element.DataContext is BasketItem basketItem) {  // Extracting the item from the DataContext of the sender.
                if (basketItem.Quantity > 1) {
                    basketItem.Quantity--;
                } else {
                    BuyCollection.Remove(basketItem);
                }
                Debug.WriteLine($"Removing 1 of: {basketItem.ItemName}");
                //increment in CSVFile
                await CSVHandler.AddAmountOfXByPID(basketItem.Item.PID, 1);
                inventoryView.LoadData();
            }
        }

        private async void ShowOutOfStockMessage(string itemName) {
            ContentDialog outOfStockDialog = new ContentDialog() {
                Title = $"{itemName} is out of stock.",
                Content = "Either Item is out of stock or you aready have to many in your basket",
                CloseButtonText = "OK"
            };
            await outOfStockDialog.ShowAsync();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) {

            if(BuyCollection.Count > 0) {
                var warningDialog = new ContentDialog {
                    Title = "Warning!",
                    Content = "You have items in your basket, please empty it before continuing",
                    PrimaryButtonText = "OK"
                };

                await warningDialog.ShowAsync();
            }else{
                Frame.Navigate(typeof(WarehousePage));
            }

        }
        private void searchButton_Click(object sender, RoutedEventArgs e) {
            inventoryView.Search(SearchTextBox.Text.ToLower());
        }
        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e) { // Allowing ENTER press for search
            if (e.Key == Windows.System.VirtualKey.Enter) {
                inventoryView.Search(SearchTextBox.Text.ToLower());
            }
        }

        private async void Buy_Basket_Button_Click(object sender, RoutedEventArgs e) {
            var checkoutDialog = new CheckoutDialog(BuyCollection) {
                PrimaryButtonText = "Buy",
                SecondaryButtonText = "Cancel"
            };
            var result = await checkoutDialog.ShowAsync();

            if (result == ContentDialogResult.Primary) {
                try {
                    if (BuyCollection.Count == 0) {
                        return;
                    }
                    CSVHandler.Checkout(BuyCollection);
                    var itemsToRemove = new List<BasketItem>(BuyCollection);
                    foreach (BasketItem item in itemsToRemove) {
                        BuyCollection.Remove(item);
                    }
                    checkoutDialog.Hide();
                }catch (Exception ex) {
                    Debug.WriteLine(ex);
                }
            }

        }
        private async void SyncDB(object sender, RoutedEventArgs e) {
            Debug.WriteLine("CALL OK");
            if(BuyCollection.Count == 0){
                await APIHandler.Refresh();
                inventoryView.LoadData();
            }else{
                ContentDialog errDialog = new ContentDialog(){
                    Title = "Warning",
                    Content = "Please empty your basket before you syncronize with the API",
                    PrimaryButtonText = "OK"
                };
                await errDialog.ShowAsync();
            }
        }
    }
}
