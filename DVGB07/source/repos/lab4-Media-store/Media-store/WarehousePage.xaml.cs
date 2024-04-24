using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Media_store {
    public sealed partial class WarehousePage : Page {

        public WarehousePage() {
            this.InitializeComponent();
            inventoryView.ItemClicked += InventoryView_ItemClicked;
        }

        private async void InventoryView_ItemClicked(object sender, InventoryItemClickEventArgs e) {
            Item clickedItem = e.ClickedItem;

            ContentDialog AddOrDeleteDialog = new ContentDialog {
                Title = "Add more or delete bulk items?",
                PrimaryButtonText = "Add",
                SecondaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };
            ContentDialogResult addOrDelete = await AddOrDeleteDialog.ShowAsync();

            AmountDialog amountDialog;

            if (addOrDelete == ContentDialogResult.Primary) {
                amountDialog = new AmountDialog(true);
            } else if (addOrDelete == ContentDialogResult.Secondary) {
                amountDialog = new AmountDialog(false);
            } else {
                AddOrDeleteDialog.Hide();
                return;
            }

            if (amountDialog == null) {
                ShowErrorMessage("The amount == NULL");
            }

            await amountDialog.ShowAsync();
            int qtyToAddOrDelete = amountDialog.Amount;

            if (clickedItem.Stock == qtyToAddOrDelete && !amountDialog.Add) {

                ContentDialog yesOrNo = new ContentDialog {
                    Title = $"Warning",
                    Content = $"You are trying to delete the whole inventory of {clickedItem.Name}. Are you sure you want to do that?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };
                ContentDialogResult result = await yesOrNo.ShowAsync();

                if (result == ContentDialogResult.Primary) {
                    string deleteResult = await CSVHandler.DeleteAmountOfXByPID(clickedItem.PID, qtyToAddOrDelete);

                    if (deleteResult != null && deleteResult.Contains("Deleted the whole inventory")) {

                        ContentDialog deleteItemResult = new ContentDialog {
                            Title = $"Delete item fully?",
                            Content = $"The stock of {clickedItem.Name} is now zero. Do you want to delete this item from the inventory completley?",
                            PrimaryButtonText = "Yes",
                            SecondaryButtonText = "No"
                        };
                        ContentDialogResult delItem = await deleteItemResult.ShowAsync();

                        if (delItem == ContentDialogResult.Primary) {
                            await CSVHandler.DeleteItemByPID(clickedItem.PID);
                        }
                    }
                }

            } else if (clickedItem.Stock < qtyToAddOrDelete && !amountDialog.Add) {
                ContentDialog deleteToMuch = new ContentDialog {
                    Title = $"Deleting too many",
                    Content = $"You are trying to delete too many items of {clickedItem.Name}. Current stock is {clickedItem.Stock}!\n Do you want to remove the item from the invetory completly?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };
                ContentDialogResult resultTooMany = await deleteToMuch.ShowAsync();

                if (resultTooMany == ContentDialogResult.Primary) {
                    await CSVHandler.DeleteItemByPID(clickedItem.PID);
                }

            } else {
                switch (addOrDelete) {
                    case ContentDialogResult.Primary:
                        await CSVHandler.AddAmountOfXByPID(clickedItem.PID, qtyToAddOrDelete);
                        break;

                    case ContentDialogResult.Secondary:
                        await CSVHandler.DeleteAmountOfXByPID(clickedItem.PID, qtyToAddOrDelete);
                        break;

                    default:
                        ShowErrorMessage("Error in addorDeleteDialog");
                        break;
                }
            }

            inventoryView.LoadData();
        }

        private async void ShowMessage(string message) {
            ContentDialog msgDialog = new ContentDialog {
                Title = "Msg",
                Content = message,
                CloseButtonText = "OK"
            };

            await msgDialog.ShowAsync();
        }
        public async void ShowErrorMessage(string message) {
            ContentDialog errorDialog = new ContentDialog {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK"
            };

            await errorDialog.ShowAsync();
        }

        private async void AddItem_Click(object sender, RoutedEventArgs e) {
            ChooseItemDialog dialog = new ChooseItemDialog();
            await dialog.ShowAsync();

            var result = dialog.SelectedItemType;

            switch (result) {
                case ItemType.Book:
                    AddBookDialog bookDialog = new AddBookDialog();
                    await bookDialog.ShowAsync();
                    break;
                case ItemType.Game:
                    AddGameDialog gameDialog = new AddGameDialog();
                    await gameDialog.ShowAsync();
                    break;
                case ItemType.Movie:
                    AddMovieDialog movieDialog = new AddMovieDialog();
                    await movieDialog.ShowAsync();
                    break;
            }
            inventoryView.LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(MainPage));
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e) {
            inventoryView.Search(SearchTextBox.Text.ToLower());
        }
        private void SearchTextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                inventoryView.Search(SearchTextBox.Text.ToLower());
            }
        }
    }
}
