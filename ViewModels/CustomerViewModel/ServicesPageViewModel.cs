using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Utilities;
using Barangay_Office.Views;
using Barangay_Office.Views.Customer;
using ZstdSharp.Unsafe;

namespace Barangay_Office.ViewModels.CustomerViewModel
{
    public partial class ServicesPageViewModel : BaseViewModel
    {

        //fields
        private string _selectedFormTypes = "Select Form Type";
        private bool _isMale;
        private bool _isFemale;
        private bool _isSingle;
        private bool _isMarried;
        private string _dataLabelText = "BirthDate";
        private DateComponent _requiredDateComponent = DateComponent.FullDate;
        private DateTime _selectedDate = DateTime.Now;
        private string _formattedDate = string.Empty;
        private string _selectedProvince = string.Empty;
        private string _selectedMunicipality = string.Empty;


        private RegionData _regionData = new();
        public ObservableCollection<string> Provinces { get; set; } = [];
        public ObservableCollection<string> Municipalities { get; set; } = [];
        public ObservableCollection<string> Barangays { get; set; } = [];



        //properties
        private Dictionary<string, bool> _entryVisibility = [];
        public Dictionary<string, bool> EntryVisibility => _entryVisibility;

        public ObservableCollection<string> FormTypes { get; } =
        [
            "Barangay Clearance",
            "Indigency Certificate",
            "Tax Certificate",
            "Business Permit"
        ];


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
        public DateComponent RequiredDateComponent
        {
            get => _requiredDateComponent;
            set
            {
                if(_requiredDateComponent != value)
                {
                    _requiredDateComponent = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if(_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    UpdateDateComponent();
                }
            }
        }
        public string FormattedDate
        {
            get => _formattedDate;
            set
            {
                if(_formattedDate != value)
                {
                    _formattedDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public string DateLabelText
        {
            get => _dataLabelText;
            set
            {
                if(_dataLabelText != value)
                {
                    _dataLabelText = value;
                    OnPropertiesChanged();
                }
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
        public bool IsSingle
        {
            get => _isSingle;
            set
            {
                if (_isSingle != value)
                {
                    _isSingle = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsMarried
        {
            get => _isMarried;
            set
            {
                if (_isMarried != value)
                {
                    _isMarried = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OnSubmit { get; }

        //constructor
        public ServicesPageViewModel()
        {
            //if (Application.Current == null)
            //{
            //    throw new InvalidOperationException("Application.Current is null. Ensure the application is properly initialized.");
            //}

            OnSubmit = new RelayCommand(async _ =>
            {
                GeneratedCertificate();
                await Shell.Current.GoToAsync(nameof(CustomerPage));
            });
            InitializeEntryVisibility();
            LoadRegionData();

        }

        private void GeneratedCertificate()
        {
            // Collect user inputs
            string name = "John Doe"; // Replace with actual entry values
            string address = "Sample Address"; // Replace with actual entry values
            string formType = SelectedFormType;
            string date = DateTime.Now.ToString("MMMM dd, yyyy");

            // Generate the certificate using CertificateGenerator
            var certificateStream = CertificateGenerator.GenerateCertificate("business_permit", name, date, formType, address);

            // Add the certificate to the centralized storage
            CertificateStorage.Certificates.Add(new CertificatesModel
            {
                Title = $"{formType} for {name}",
                CertificateImage = certificateStream
            });
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
                "Fistname",
                "Lastname",
                "Middlename",
                "Address",
                "Citizenship",
                "Height",
                "Weight",
                "Date",
                "Gender",
            })
            
            {
                _entryVisibility[key] = true;
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
            _entryVisibility = SelectedFormType switch
            {
                "Tax Certificate" => new Dictionary<string, bool>
                {
                    ["ICR"] = true,
                    ["Height"] = true,
                    ["Weight"] = true,
                    ["Date"] = true,

                },
                "Barangay Clearance" => new Dictionary<string, bool>
                {

                    ["Firstname"] = true,
                    ["Lastname"] = true,
                    ["Middlename"] = true,
                    ["Citizenship"] = true,
                    ["Address"] = true,
                },

                "Indigency Certificate" => new Dictionary<string, bool>{

                    ["Height"] = true
                },

                "Business Permit" => new Dictionary<string, bool>
                {

                    ["Weight"] = true
                },
                _ => _entryVisibility
            };
            DateLabelText = SelectedFormType switch
            {
                "Tax Certificate" => "Year:",
                "Barangay Clearance" => "Birthdate:",
                "Indigency Certificate" => "Birthdate:",
                "Business Permit" => "Birthdate:",
                _ => "Birthdate:"
            };
            
            OnPropertyChanged(nameof(DateLabelText));
            OnPropertyChanged(nameof(EntryVisibility));
        }
        private void UpdateDateComponent()
        {
            FormattedDate = RequiredDateComponent switch
            {
                DateComponent.Day => FormattedDate = SelectedDate.Day.ToString(),
                DateComponent.Month => FormattedDate = SelectedDate.ToString("MMMM"),
                DateComponent.Year => FormattedDate = SelectedDate.ToString(),
                DateComponent.FullDate or _ => FormattedDate = SelectedDate.ToString("MM/dd/yyyy"),
            };
        }

        public enum DateComponent
        {
            FullDate,
            Day,
            Month,
            Year
        }
    }

}
