using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Media_store {
    public sealed partial class AmountDialog : ContentDialog {
        public int Amount { get; private set; }
        public bool Add;
        public AmountDialog(bool add) {
            this.InitializeComponent();

            this.PrimaryButtonText = (add == true) ? "Add" : "Delete";
            this.Title = (add == true) ? "Enter the amount you want to add" : "Enter the amount you want to delete";
            Add = (add == true);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (!int.TryParse(AmountTextBox.Text, out int amount2Add)) {
                ErrorMessage.Visibility = Visibility.Visible;
                args.Cancel = true;
            }else{
                ErrorMessage.Visibility = Visibility.Collapsed;
                Amount = amount2Add;
            }    
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
        
        }
    }
}
