using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class ServicesPageViewModel : BaseViewModel
    {
        private string _selectedFormTypes = "Select Form Type";
        private bool _isMale;
        private bool _isFemale;
        private readonly Dictionary<string, bool> _entryVisibility = new();

        public ObservableCollection<string> FormTypes { get; } = new()
        {
            "Barangay Clearance",
            "Indigency Certificate",
            "Tax Certificate",
            "Business Permit"
        };

        public string SelectedFormType
        {
            get => _selectedFormTypes ?? "Select Form Type";
            set
            {
                _selectedFormTypes = value;
                OnPropertyChanged();
                UpdateEntryVisibility();
            }
        }

        public bool IsEntryVisible(string entryName)
        {
            return _entryVisibility.TryGetValue(entryName, out var isVisible) && isVisible;
        }

        public bool IsMale
        {
            get => _isMale;
            set
            {
                _isMale = value;
                OnPropertyChanged();
            }
        }

        public bool IsFemale
        {
            get => _isFemale;
            set
            {
                _isFemale = value;
                OnPropertyChanged();
            }
        }

        public ICommand GotoPaymentCommand { get; }


        public ServicesPageViewModel()
        {
            if (Application.Current == null)
            {
                throw new InvalidOperationException("Application.Current is null. Ensure the application is properly initialized.");
            }

            GotoPaymentCommand = new RelayCommand(_ => Shell.Current.GoToAsync(nameof(PaymentInfoPage)));
            InitializeEntryVisibility();
        }


        private void InitializeEntryVisibility()
        {
            _entryVisibility["ICR"] = false;
            _entryVisibility["Citizenship"] = false;
        }

        private void UpdateEntryVisibility()
        {
            //Reset all entries to false
            foreach (var key in _entryVisibility.Keys.ToList())
            {
                _entryVisibility[key] = false;
            }

            //update the visibility depending on the seelected form type
            if(SelectedFormType == "Tax Certificate")
            {
                _entryVisibility["ICR"] = true;
            }else if (SelectedFormType == "Barangay Clearance")
            {
                _entryVisibility["Citizenship"] = true;
            }

            foreach(var key in _entryVisibility.Keys)
            {
                OnPropertyChanged($"Is{key}Visible");
            }

        }
    }
}
