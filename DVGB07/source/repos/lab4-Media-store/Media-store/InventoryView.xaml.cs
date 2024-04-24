using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Media_store {
    public class InventoryItemClickEventArgs : EventArgs {
        public Item ClickedItem { get; }
        public InventoryItemClickEventArgs(Item item) {
            this.ClickedItem = item;
        }
    }

    public sealed partial class InventoryView : UserControl {

        public ObservableCollection<Item> InventoryItems;

        public event EventHandler<InventoryItemClickEventArgs> ItemClicked;

        public InventoryView() {

            this.InitializeComponent();

            InventoryItems = new ObservableCollection<Item>();

            LoadData();

        }
        private void OnItemClicked(Item item) {
            ItemClicked?.Invoke(this, new InventoryItemClickEventArgs(item));
        }
        private void ListViewItem_Tapped(object sender, TappedRoutedEventArgs e) {
            if (sender is FrameworkElement element && element.DataContext is Item item) {
                OnItemClicked(item);
            }
        }
        public async void LoadData() {
            try {
                InventoryItems.Clear();

                List<Item> items = await CSVHandler.ReadDataFromCSVAsync();

                foreach (var item in items) {
                    InventoryItems.Add(item);
                }

                BookList.ItemsSource = InventoryItems.Where(item => item is Book);
                GameList.ItemsSource = InventoryItems.Where(item => item is Game);
                MovieList.ItemsSource = InventoryItems.Where(item => item is Movie);

            } catch (Exception ex) {
                Debug.WriteLine($"DEBUG FROM LoadDAta: {ex}");
            }
        }
        public void Search(string StringToFind) {
            try {
                ObservableCollection<Item> filteredItems = new ObservableCollection<Item>();
                Debug.WriteLine("SEARCHING!!!");
                if (!string.IsNullOrWhiteSpace(StringToFind)) {
                    foreach (var item in InventoryItems) {

                        if (item.Name.ToLower().Contains(StringToFind)) {
                            filteredItems.Add(item);
                        }

                        if (int.TryParse(StringToFind, out int result)) {
                            if (item.PID == result) {
                                filteredItems.Add(item);
                            }
                        }
                        if (item is Book bookItem) {
                            if (bookItem.Author.ToLower().Contains(StringToFind) ||
                                bookItem.Genre.ToLower().Contains(StringToFind) ||
                                bookItem.Language.ToLower().Contains(StringToFind) ||
                                bookItem.Format.ToLower().Contains(StringToFind)) {

                                filteredItems.Add(bookItem);
                            }
                        }

                        if (item is Game gameItem) {
                            if (gameItem.Platform.ToLower().Contains(StringToFind)) {
                                filteredItems.Add(gameItem);
                            }
                        }

                        if (item is Movie movieItem) {
                            if (movieItem.Format.ToLower().Contains(StringToFind)) {
                                filteredItems.Add(movieItem);
                            }
                        }
                    }
                    BookList.ItemsSource = filteredItems.Where(item => item is Book);
                    GameList.ItemsSource = filteredItems.Where(item => item is Game);
                    MovieList.ItemsSource = filteredItems.Where(item => item is Movie);
                } else {
                    LoadData();
                }

            } catch (Exception ex) {
                Debug.WriteLine($"Error from Search {ex}");
            }

        }
    }
}
