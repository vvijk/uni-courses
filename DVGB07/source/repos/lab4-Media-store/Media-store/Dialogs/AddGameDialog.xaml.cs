using System;
using System.Collections.Generic;
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
    public sealed partial class AddGameDialog : ContentDialog
    {
        public AddGameDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialogAddGameButton_Click(object sender, RoutedEventArgs e){
            string name = AddGameName.Text;
            string price = AddGamePrice.Text;
            string amount= AddGameAmount.Text;
            string platform= AddGamePlatform.Text;

            if (string.IsNullOrEmpty(name)){ // TODO: Handle whitepsace and specialchar! rn it doesnt allow
                AddGameErrorMessage.Text = "Name cannot be left empty.";
                return;
            }else if (!int.TryParse(price, out int price1) || price1 < 0){
                AddGameErrorMessage.Text = "Price must be a number and greater than 0.";
                return;
            }else if (!int.TryParse(amount, out int amountToAdd) || amountToAdd < 0){
                AddGameErrorMessage.Text = "Amount must be a number and greater than 0.";
                return;
            }else{
                Task<int> task = CSVHandler.CreateUniquePIDAsync();
                int newPID = await task;

                Game newGame = new Game(newPID, name, int.Parse(price), amountToAdd, platform);

                CSVHandler.AddDataToCSVAsync(newGame, int.Parse(amount));

                AddGameErrorMessage.Text = "GAME WAS ADDED";

                this.Hide();
            }
            AddGameErrorMessage.Text = "";
        }

        private void ContentDialogCancelGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
