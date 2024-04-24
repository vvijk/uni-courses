using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Media_store {
    public sealed partial class AddBookDialog : ContentDialog {
        public AddBookDialog() {
            this.InitializeComponent();
            Title = "Add a new book";

        }
        private async void ContentDialogAddBookButton_Click(object sender, RoutedEventArgs e) {
            try {
                string name = AddBookName.Text;
                string price = AddBookPrice.Text;
                string author = AddBookAuthor.Text;
                string genre = AddBookGenre.Text;
                string format = AddBookFormat.Text;
                string language = AddBookLanguage.Text;
                string amount = AddBookAmount.Text;

                if (string.IsNullOrEmpty(name)) {
                    AddBookErrorMessage.Text = "Name cannot be left empty.";
                    return;
                } else if (!int.TryParse(price, out int price1) || price1 < 0) {
                    AddBookErrorMessage.Text = "Price must be a number and greater than 0.";
                    return;
                } else if (!int.TryParse(amount, out int amountToAdd) || amountToAdd < 0) {
                    AddBookErrorMessage.Text = "Amount must be a number and greater than 0.";
                    return;
                } else {
                    Task<int> task = CSVHandler.CreateUniquePIDAsync();
                    int newPID = await task;

                    Book newBook = new Book(newPID, name, int.Parse(price), amountToAdd, author, genre, format, language);
                
                    CSVHandler.AddDataToCSVAsync(newBook, int.Parse(amount));

                    Debug.WriteLine($"Book: {newBook.Name} was added?");
                    this.Hide();
                }
                AddBookErrorMessage.Text = "";
            } catch (Exception ex) {
                Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private void ContentDialogCancelBookButton_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }

    }
}
