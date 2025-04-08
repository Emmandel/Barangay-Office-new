using System.Collections.ObjectModel;
using System.Windows.Input;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class CustomerPageViewModel: BaseViewModel
    {
        
        public string SelectedButton
        {
            get => NavigationState.SelectedButton;
            set
            {
                if(NavigationState.SelectedButton != value)
                {
                    NavigationState.SelectedButton = value;
                    OnPropertyChanged();
                }
            }
        }

        public static class NavigationState
        {
            public static string SelectedButton { get; set; } = "CustomerPage"; // Default selected button
        }


        public ICommand NavigateCommand { get; }

        public CustomerPageViewModel()
        {

            NavigateCommand = new RelayCommand(async param =>
            {
                if (param is string page && !string.IsNullOrWhiteSpace(page))
                {
                    SelectedButton = page;
                    await Shell.Current.GoToAsync($"///{page}");
                }
            });
        }

    }
}
