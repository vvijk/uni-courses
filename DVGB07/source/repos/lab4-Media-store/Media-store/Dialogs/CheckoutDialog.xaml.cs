using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Media_store {
    public sealed partial class CheckoutDialog : ContentDialog {
        public ObservableCollection<BasketItem> BuyCollection { get; set; }
        public string TotalPrice { get; set; }

        public CheckoutDialog(ObservableCollection<BasketItem> buyCollection) {
            this.InitializeComponent();
            BuyCollection = buyCollection;
            CalcTotalPrice();
        }

        private void CalcTotalPrice() {
            double sum = 0;
            foreach (var item in BuyCollection) {
                sum += item.Item.Price * item.Quantity;
            }
            TotalPrice = sum.ToString();
        }

        //private void Buy_Button_Click(object sender, RoutedEventArgs e) {
        //    try {
        //        if (BuyCollection.Count == 0) {
        //            return;
        //        }
        //        CSVHandler.BuyBasket(BuyCollection);
        //
        //        var itemsToRemove = new List<BasketItem>(BuyCollection);
        //
        //        foreach (BasketItem item in itemsToRemove) {
        //            BuyCollection.Remove(item);
        //        }
        //        this.Hide();
        //    }catch (Exception ex) {
        //        Debug.WriteLine($"Error from Buy_Buton_Click{ex}");
        //    }
        //}
        //
        //private void Cancel_Button_Click(object sender, RoutedEventArgs e) {
        //    this.Hide();
        //}

    }
}
