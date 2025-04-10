using System.Collections.ObjectModel;
using System.Windows.Input;
using Barangay_Office.Utilities;
using Barangay_Office.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Barangay_Office.ViewModels
{
    public class CustomerPageViewModel: BaseViewModel
    {
        private string _selectedButton = string.Empty;
        private bool _isNavigating;
        public string SelectedButton
        {
            get => _selectedButton;
            set {
                _selectedButton = value;
                OnPropertyChanged();
            }
        }

        public static class NavigationState
        {
            public static string SelectedButton { get; set; } = "CustomerPage"; // Default selected button
        }


        public ICommand NavigateCommand { get; }

        public CustomerPageViewModel()
        {

            NavigateCommand = new Command<string>(async (page) => await NavigateToPageAsync(page));
            Shell.Current.Navigated += OnShellNavigated;
        }

        private async Task NavigateToPageAsync(string page)
        {
            if (_isNavigating) return;
            _isNavigating = true;

            try
            {

                // Simulate navigation delay (replace with actual navigation logic)
                await Shell.Current.GoToAsync($"//{page}");

                // Update the SelectedButton after navigation is complete
                SelectedButton = page;
            }
            finally
            {
                _isNavigating = false;
            }
        }

        private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            // Update SelectedButton based on the current location
            var currentPage = e.Current.Location.OriginalString;

            // Extract the page name from the URI (e.g., "CustomerPage" from "//CustomerPage")
            var pageName = currentPage.TrimStart('/');

            // Update the SelectedButton to match the current page
            SelectedButton = pageName;
        }

    }
}
