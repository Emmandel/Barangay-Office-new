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

        public ICommand GotoPaymentCommand { get; }

        public ICommand ToggleTheme { get; }

        public ServicesPageViewModel()
        {
            GotoPaymentCommand = new RelayCommand(_ => Shell.Current.GoToAsync(nameof(PaymentInfoPage)));
            InitializeEntryVisibility();

            ToggleTheme = new RelayCommand(Ttheme =>
            {
                if (Application.Current.UserAppTheme == AppTheme.Light)
                {
                    SetTheme(AppTheme.Dark);
                }
                else
                {
                    SetTheme(AppTheme.Light);
                }
            });
        }


        //toggle theme
        private void SetTheme(AppTheme theme)
        {
            Application.Current.UserAppTheme = theme;
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
