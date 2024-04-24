using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace Media_store
{
    public sealed partial class AddMovieDialog : ContentDialog
    {
        public AddMovieDialog()
        {
            this.InitializeComponent();
        }
        private async void ContentDialogAddMovieButton_Click(object sender, RoutedEventArgs e){
            string name = AddMovieName.Text;
            string price = AddMoviePrice.Text;
            string format = AddMovieFormat.Text;
            string length = AddMovieLength.Text;
            string amount = AddMovieAmount.Text;

            if (string.IsNullOrEmpty(name)){
                AddMovieErrorMessage.Text = "Name cannot be left empty.";
                return;
            }else if (!int.TryParse(price, out int price1) || price1 < 0){
                AddMovieErrorMessage.Text = "Price must be a number and greater than 0.";
                return;
            }else if (!int.TryParse(amount, out int amountToAdd) || amountToAdd < 0){
                AddMovieErrorMessage.Text = "Amount must be a number and greater than 0.";
                return;
            } else if (!int.TryParse(length, out int lengthToAdd) || lengthToAdd < 0) {
                AddMovieErrorMessage.Text = "Length must be a number and greater than 0.";
                return;
            } else{
                Task<int> task = CSVHandler.CreateUniquePIDAsync();
                int newPID = await task;
                
                Movie newMovie = new Movie(newPID, name, int.Parse(price), amountToAdd, format, int.Parse(length));

                CSVHandler.AddDataToCSVAsync(newMovie, int.Parse(amount));

                AddMovieErrorMessage.Text = "MOVIE WAS ADDED";
                this.Hide();
            }
            AddMovieErrorMessage.Text = "";
        }

        private void ContentDialogCancelMovieButton_Click(object sender, RoutedEventArgs e){
            this.Hide();
        }
    }
}
