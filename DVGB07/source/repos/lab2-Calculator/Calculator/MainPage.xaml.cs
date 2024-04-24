using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;


namespace Calculator {
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }

        private const string EqualOperator = "=";
        private const string AdditionOperator = "+";
        private const string DivisionOperator = "/";
        private const string SubtractionOperator = "-";
        private const string MultiplicationOperator = "*";

        private string firstInput, latestInput;
        private string currentOperator, prevOperator;
        private int currentResult;

        private void Button_Click_Divide(object sender, RoutedEventArgs e) { SetOperator(DivisionOperator); }
        private void Button_Click_Addition(object sender, RoutedEventArgs e) { SetOperator(AdditionOperator); }
        private void Button_Click_Mult(object sender, RoutedEventArgs e) { SetOperator(MultiplicationOperator); }
        private void Button_Click_Subtraction(object sender, RoutedEventArgs e) { SetOperator(SubtractionOperator); }

        private void Number_Button(object sender, RoutedEventArgs e) {

            if (sender is Button button) {
                string buttonText = button.Content.ToString();

                if (display.Text == "0" || currentOperator == EqualOperator) {
                    Clear_Button(sender, e);
                    display.Text = buttonText;

                } else {
                    display.Text += buttonText;
                }

            }
        }

        private void SetPreviousDisplay(string input, string operatorToShow, int x) {
            if (x == -1) {
                displayPrevious.Text = input + " " + operatorToShow;
            } else {
                displayPrevious.Text = input + " " + operatorToShow + " " + x;
            }
        }

        private void SetDisplay(string input) {
            display.Text = input;
        }

        private int PerformCalculation(int left, int right, string operatorSymbol) {
            try {
                checked {

                    switch (operatorSymbol) {
                        case "+":
                            return left + right;
                        case "-":
                            return left - right;
                        case "*":
                            return left * right;
                        case "=":
                            return 0;
                        case "/":
                            if (right != 0) {
                                return left / right;
                            } else {
                                throw new InvalidOperationException("Division by zero is not allowed");
                            }
                        default:
                            throw new InvalidOperationException("Unsupported operator.");
                    }
                }
            } catch (OverflowException) {
                throw new InvalidOperationException("Result is to big");
            }
        }

        private void SetOperator(string operatorSymbol) {

            if (string.IsNullOrEmpty(display.Text)){
                currentOperator = operatorSymbol;
                SetPreviousDisplay(firstInput, currentOperator, -1);
            }else{


                if (currentOperator == EqualOperator){
                    SetPreviousDisplay(currentResult.ToString(), operatorSymbol, -1);

                }else if (firstInput !=  null && !string.IsNullOrEmpty(currentOperator) && !string.IsNullOrEmpty(display.Text)){
                    Button_Click_Equal(null, null);
                }
            
                currentOperator = operatorSymbol;
            
                firstInput = display.Text;
            
                SetPreviousDisplay(firstInput, operatorSymbol, -1);

                display.Text = string.Empty;
            
            }
        }

        private async void Button_Click_Equal(object sender, RoutedEventArgs e) {
            try {
                int left, right;

                // Updating left and right accordingly to multiple "=" press's in a row
                if (currentOperator == EqualOperator) {
                    currentOperator = prevOperator;
                    left = currentResult;
                    right = Convert.ToInt32(latestInput);
                } 
                else {
                    left = Convert.ToInt32(firstInput);
                    right = Convert.ToInt32(display.Text);
                    prevOperator = currentOperator;
                    latestInput = right.ToString();                    
                }

                currentResult = PerformCalculation(left, right, currentOperator);
                SetDisplay(Convert.ToString(currentResult));
                SetPreviousDisplay(Convert.ToString(left), currentOperator, right);
                currentOperator = EqualOperator;
            } catch (Exception ex) {
                await ShowErrorMessage(ex.Message);
            }
        }

        private async Task ShowErrorMessage(string errorMessage) {
            var dialog = new MessageDialog(errorMessage);
            dialog.Title = "Error";
            await dialog.ShowAsync();
        }

        private void Clear_Button(object sender, RoutedEventArgs e) {
            display.Text = "";
            displayPrevious.Text = "";
            
            currentResult = 0;
            firstInput = "";
            latestInput = "";
            
            currentOperator = "";
            prevOperator = "";
        }

    }
}