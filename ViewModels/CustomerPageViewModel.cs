using System.Collections.ObjectModel;
using System.Windows.Input;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class CustomerPageViewModel: BaseViewModel
    {
        private string _selectedFormTypes;

        public ObservableCollection<string> FormTypes { get; } = new()
        {
            "Barangay Clearance",
            "Indigency Certificate",
            "Business Permit"
        };

        public string SelectedFormType
        {
            get => _selectedFormTypes ?? "Select Form Type";
            set
            {
                _selectedFormTypes = value;
                OnPropertyChanged();
            }
        }

        public ICommand GotoPaymentCommand { get; }
        public ICommand NavigateCommand { get; }

        public CustomerPageViewModel()
        {
            GotoPaymentCommand = new RelayCommand(_ => Shell.Current.GoToAsync(nameof(PaymentInfoPage)));
            NavigateCommand = new RelayCommand(async param =>
            {
                if (param is string page && !string.IsNullOrWhiteSpace(page))
                {
                    await Shell.Current.GoToAsync($"///{page}");
                }
            });
        }

    }
}
