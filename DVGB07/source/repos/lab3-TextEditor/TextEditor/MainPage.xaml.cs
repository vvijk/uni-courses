using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Resource show a Dialog when closing the window
// https://stackoverflow.com/questions/62910280/is-it-possible-to-pop-up-my-dialog-box-when-click-the-close-icon-on-the-upper-ri

namespace TextEditor
{
    public sealed partial class MainPage : Page
    {
        private string firstContent = "";
        private bool hasBeenEdited = false, newFile = true;
        private StorageFile FILE;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().Title = "unnamed_file";
            TextArea.TextChanged += TextArea_TextChanged; //Attatch an event handler to TextArea to check if the text has been edited
            FILE = null;
            //Had to add this to Package.appxmanifest:
            // <rescap:Capability Name="confirmAppClose"/>
            //xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
            // IgnorableNamespaces = "uap mp rescap"
            SetupExitHandling();
        }

        private void SetupExitHandling() {
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested +=
                async (sender, args) => await HandleExitAttempt(args);
        }

        private void TextCounting() {
            string text = TextArea.Text;

            int charsWithSpace = text.Length;
            int charsWithOutSpace = text.Replace(" ", "").Length;

            char[] toDelete = { ' ', '\t', '\n', '\r' };
            int words = text.Split(toDelete, StringSplitOptions.RemoveEmptyEntries).Length;
            
            string[] toDelete1= { "\r\n", "\r", "\n" };
            int rows = text.Split(toDelete1, StringSplitOptions.None).Length;

            StatusText0.Text = $"Characters without spaces: {charsWithOutSpace}";
            StatusText1.Text = $"Characters with spaces: {charsWithSpace}";
            StatusText2.Text = $"Words: {words}";
            StatusText3.Text = $"Rows: {rows}";
        }

        private void TextArea_KeyDown(object sender, KeyRoutedEventArgs e) { TextCounting();}

        private async Task ShowErrorMessage(string errorMessage) {
            var dialog = new MessageDialog(errorMessage);
            dialog.Title = "Error";
            await dialog.ShowAsync();
        }

        private async Task SaveDialog() {
            ContentDialog dialog = new ContentDialog {
                Title = "Unsaved Changes",
                Content = "Do you want to save before closing?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary) {
                if (newFile){
                    await SaveAsAsync();
                }
                else{
                    await Save_Async();
                }
            }else if (result == ContentDialogResult.Secondary) {
                hasBeenEdited = false;
                newFile = true;
                firstContent = "";
                CheckEdit();
            }

        }
        private async Task HandleExitAttempt(SystemNavigationCloseRequestedPreviewEventArgs args) {

            if (hasBeenEdited) {
                args.Handled = true; // Stop the closing operation
                await SaveDialog();
                CoreApplication.Exit(); // Hard exit..
            }
        }
        private async void OpenFile(object sender, RoutedEventArgs e) {
            try {

                if(hasBeenEdited) {
                    await SaveDialog();
                }

                FileOpenPicker OpenPicker = new FileOpenPicker();
                OpenPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                OpenPicker.FileTypeFilter.Add(".txt");

                FILE =  await OpenPicker.PickSingleFileAsync();

                if (FILE != null) {
                    TextArea.Text = await FileIO.ReadTextAsync(FILE);
                    firstContent = TextArea.Text;
                    ApplicationView.GetForCurrentView().Title = $"{FILE.Name}";
                    TextCounting();
                    newFile = false;
                }

            }catch (Exception ex) {
                await ShowErrorMessage(ex.Message);
            }

        }
        private void CheckEdit() {
            string currTitle = ApplicationView.GetForCurrentView().Title;
            bool isStarPresent = currTitle.EndsWith("*");

            hasBeenEdited = firstContent != TextArea.Text;

            if (hasBeenEdited && !isStarPresent) {
                ApplicationView.GetForCurrentView().Title = $"{currTitle}*";
            } else if (!hasBeenEdited && isStarPresent){
                ApplicationView.GetForCurrentView().Title = currTitle.TrimEnd('*');
            }
        }
        private void TextArea_TextChanged(object sender, RoutedEventArgs e) {
            CheckEdit();
        }
        
        private async Task SaveAsAsync() {
            try {
                FileSavePicker picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.Desktop;
                picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
                picker.SuggestedFileName = "unnamed_file";

                FILE = await picker.PickSaveFileAsync();

                // Resource: https://learn.microsoft.com/en-us/windows/uwp/files/quickstart-save-a-file-with-a-picker
                if (FILE != null) {
                    // Prevent updates to the remote version of the file until
                    // we finish making changes and call CompleteUpdatesAsync.
                    CachedFileManager.DeferUpdates(FILE);

                    // Write TextArea.Text to file
                    await FileIO.WriteTextAsync(FILE, TextArea.Text);

                    //await and let Windows know we are done so other apps can use it
                    Windows.Storage.Provider.FileUpdateStatus status =
                        await CachedFileManager.CompleteUpdatesAsync(FILE);
                    
                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete) {
                        ApplicationView.GetForCurrentView().Title = FILE.Name;
                        hasBeenEdited = false;
                        firstContent = TextArea.Text;
                        CheckEdit();
                    } else {
                       CommandContent0.Text = FILE.Name + " couldn't be saved.";
                    }
                }

                newFile = false;

            } catch (Exception ex) {
                await ShowErrorMessage($"{ex.Message}");
            }

        }
        private async Task Save_Async(){
            try {
                Debug.WriteLine($"We inside Save_Async() and newFile: {newFile}");

                if (newFile){
                    await SaveAsAsync();
                }
                else
                {
                    if(FILE != null){
                        await FileIO.WriteTextAsync(FILE, TextArea.Text);
                        ApplicationView.GetForCurrentView().Title = $"{FILE.Name}";
                        firstContent = TextArea.Text;
                    }else{
                        await ShowErrorMessage("FILE == null");
                    }
                }
                CheckEdit();
            }
            catch ( Exception ex) {
                Debug.WriteLine($"Error from Save_ASync. ex: {ex}");
            }

        }
        private async void SaveAsButton(object sender, RoutedEventArgs e) { await SaveAsAsync(); }
        private async void SaveButton(object sender, RoutedEventArgs e) { await Save_Async(); }


        private async void Clear(object sender, RoutedEventArgs e) {
            MessageDialog msg = new MessageDialog("Do you really want to erase all your beautiful work?");
            msg.Commands.Add(new UICommand("Yes"));
            msg.Commands.Add(new UICommand("No"));

            var result = await msg.ShowAsync();

            if (result.Label == "Yes") {
                newFile = true;
                TextArea.Text = string.Empty;
                firstContent = string.Empty;
                ApplicationView.GetForCurrentView().Title = "unnamed_file";
            }
        }

        // Version 3. Drag N' Drop handling  
        // Resource: https://learn.microsoft.com/en-us/windows/apps/design/input/drag-and-drop#handle-the-dragover-event
        private void TextArea_DragEnter(object sender, DragEventArgs e) {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void TextArea_Drop(object sender, DragEventArgs e) {
            bool ctrlKeyDrag = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control) == CoreVirtualKeyStates.Down;
            bool shiftKeyDrag = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) == CoreVirtualKeyStates.Down;

            if (e.DataView.Contains(StandardDataFormats.StorageItems)) {
                Debug.WriteLine($"newFile: {newFile})");

                var items = await e.DataView.GetStorageItemsAsync();
                FILE = items[0] as StorageFile;
                string text = await FileIO.ReadTextAsync(FILE);

                if (ctrlKeyDrag) {
                    TextArea.Text += text;
                }else if (shiftKeyDrag) {
                    TextArea.Text = TextArea.Text.Insert(TextArea.SelectionStart, text);
                }else {
                    if (hasBeenEdited) {
                        await SaveDialog();
                    }
                    TextArea.Text = text;
                }

                firstContent = text;
                if (FILE != null) {
                    ApplicationView.GetForCurrentView().Title = FILE.Name;
                }

                newFile = true;
            }
            TextCounting();
        }
    }
}
