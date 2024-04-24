using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JuansLottery {
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            fem.Text = "";
            sex.Text = "";
            sju.Text = "";

            Button spelaButton = (Button)sender;
            spelaButton.IsEnabled = false;

            List<int> numbers = new List<int>();

            //Check integer input
            for (int i = 0; i <= 6; i++) {
                TextBox textBox = (TextBox)FindName($"lott{i}");
                if (int.TryParse(textBox.Text, out int number) && number > 0 && number <= 35) {
                    numbers.Add(number);
                } else {
                    // Invalid input, show an error or take appropriate action
                    await ShowErrorMessage($"Invalid input in ticket: {i+1}. Please enter a valid integer between 1 and 35.");
                    spelaButton.IsEnabled = true;
                    return;
                }
            }

            // Check for duplicate inputs
            if (numbers.Distinct().Count() != numbers.Count) {
                await ShowErrorMessage("Duplicate inputs are not allowed. Please enter unique numbers.");
                spelaButton.IsEnabled = true;
                return;
            }

            // Validate 
            if (int.TryParse(antalDragningar.Text, out int dragningar) && dragningar > 0 && dragningar <= 999999) {

                fem.Text = CalculateResult(numbers, dragningar, 5).ToString();
                sex.Text = CalculateResult(numbers, dragningar, 6).ToString();
                sju.Text = CalculateResult(numbers, dragningar, 7).ToString();

                spelaButton.IsEnabled = true;
            } else {
                await ShowErrorMessage("Number of takes needs to be a number between 0 and 999'999");
                spelaButton.IsEnabled = true;
                return;
            }
        }

        private int CalculateResult(List<int> userNumbers, int numDrawings, int requiredMatches) {
            int totalWins = 0;

            for (int drawing = 1; drawing <= numDrawings; drawing++) {
                List<int> drawingNumbers = GenerateRandomNumbers();

                int matches = CountMatches(userNumbers, drawingNumbers);

                if (matches == requiredMatches) {
                    totalWins++;
                }
            }

            return totalWins;
        }

        private List<int> GenerateRandomNumbers() {
            List<int> randomNumbers = new List<int>();
            Random random = new Random();

            for (int i = 0; i < 7; i++) {
                int randomNumber;

                do {
                    randomNumber = random.Next(1, 36);
                } while (randomNumbers.Contains(randomNumber));
                    
                    randomNumbers.Add(randomNumber);
            }

            return randomNumbers;
        }

        private int CountMatches(List<int> userNumbers, List<int> drawingNumbers) {
            return userNumbers.Intersect(drawingNumbers).Count();
        }

        private async Task ShowErrorMessage(string errorMessage) {
            var dialog = new MessageDialog(errorMessage);
            dialog.Title = "Error";
            await dialog.ShowAsync();
        }



    }
}
