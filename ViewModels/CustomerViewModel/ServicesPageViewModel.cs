using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels.CustomerViewModel
{
    public partial class ServicesPageViewModel : BaseViewModel
    {

        //fields
        private string _selectedFormTypes = "Select Form Type";
        private bool _isMale;
        private bool _isFemale;

        private RegionData _regionData = new RegionData();
        public ObservableCollection<string> Provinces { get; set; } = new();
        public ObservableCollection<string> Municipalities { get; set; } = new();
        public ObservableCollection<string> Barangays { get; set; } = new();



        //properties
        private readonly Dictionary<string, bool> _entryVisibility = new();
        public Dictionary<string, bool> EntryVisibility => _entryVisibility;

        public ObservableCollection<string> FormTypes { get; } = new()
        {
            "Barangay Clearance",
            "Indigency Certificate",
            "Tax Certificate",
            "Business Permit"
        };


        private string _selectedProvince = string.Empty;
        public string SelectedProvince
        {
            get => _selectedProvince;
            set
            {
                if (_selectedProvince != value)
                {
                    _selectedProvince = value;
                    OnPropertyChanged(nameof(SelectedProvince));
                    LoadMunicipalities();
                }
            }
        }

        private string _selectedMunicipality = string.Empty;
        public string SelectedMunicipality
        {
            get => _selectedMunicipality;
            set
            {
                if (_selectedMunicipality != value)
                {
                    _selectedMunicipality = value;
                    OnPropertyChanged(nameof(SelectedMunicipality));
                    LoadBarangays();
                }
            }
        }


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




        public bool IsMale
        {
            get => _isMale;
            set
            {
                if (_isMale != value)
                {
                    _isMale = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsFemale
        {
            get => _isFemale;
            set
            {
                if (_isFemale != value)
                {
                    _isFemale = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GotoPaymentCommand { get; }

        //constructor
        public ServicesPageViewModel()
        {
            //if (Application.Current == null)
            //{
            //    throw new InvalidOperationException("Application.Current is null. Ensure the application is properly initialized.");
            //}

            GotoPaymentCommand = new RelayCommand(_ => Shell.Current.GoToAsync(nameof(PaymentInfoPage)));
            InitializeEntryVisibility();
            LoadRegionData();

        }

        //methods
        private async void LoadRegionData()
        {
            // Open the file from the app package
            using var stream = await FileSystem.OpenAppPackageFileAsync("regions.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            // Deserialize the JSON content
            var deserializedData = JsonSerializer.Deserialize<RegionData>(json);
            if (deserializedData != null)
            {
                _regionData = deserializedData;

                Provinces.Clear();
                if (_regionData.Provinces != null) // Check for null before iterating
                {
                    foreach (var province in _regionData.Provinces)
                    {
                        if (!string.IsNullOrEmpty(province.ProvinceName))
                        {
                            Provinces.Add(province.ProvinceName);
                        }
                    }
                }
            }
            else
            {
                _regionData = new RegionData(); // Fallback to an empty RegionData object
            }
        }

        private void LoadMunicipalities()
        {
            Municipalities.Clear();
            Barangays.Clear();
            if (_regionData.Provinces != null && _regionData.Provinces.Count != 0)
            {

                var province = _regionData.Provinces?.FirstOrDefault(p => p.ProvinceName == SelectedProvince);
                if (province != null)
                {
                    foreach (var mun in province.Municipalities ?? Enumerable.Empty<Municipality>())
                    {
                        if (!string.IsNullOrEmpty(mun.MunicipalityName)) 
                        {
                            Municipalities.Add(mun.MunicipalityName);
                        }
                    }
                }
            }
        }

        private void LoadBarangays()
        {
            Barangays.Clear();

            var province = _regionData.Provinces?.FirstOrDefault(p => p.ProvinceName == SelectedProvince);
            var municipality = province?.Municipalities?.FirstOrDefault(m => m.MunicipalityName == SelectedMunicipality);
            if (municipality != null && municipality.Barangays != null)
            {
                foreach (var brgy in municipality.Barangays)
                {
                    Barangays.Add(brgy);
                }
            }
        }




        //these are the default entries hidden meaning all entries shown are common on all papers
        private void InitializeEntryVisibility()
        {
            foreach (var key in new[] {
                "ICR",
                "Citizenship",
                "Height",
                "Weight" })
            {
                _entryVisibility[key] = false;
            }
        }

        private void UpdateEntryVisibility()
        {
            //Reset all entries to false
            foreach (var key in _entryVisibility.Keys.ToList())
            {
                _entryVisibility[key] = false;
            }

            // Update visibility based on the selected form type
            switch (SelectedFormType)
            {
                case "Tax Certificate":
                    _entryVisibility["ICR"] = true;
                    break;
                case "Barangay Clearance":
                    _entryVisibility["Citizenship"] = true;
                    break;
                case "Indigency Certificate":
                    _entryVisibility["Height"] = true;
                    break;
                case "Business Permit":
                    _entryVisibility["Weight"] = true;
                    break;
            }

            OnPropertyChanged(nameof(EntryVisibility));
        }
    }

}
