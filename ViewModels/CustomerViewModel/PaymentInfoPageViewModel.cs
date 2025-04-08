using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barangay_Office.ViewModels
{
    public class PaymentInfoPageViewModel : BaseViewModel
    {
        private string _selectedModeOfPayment = string.Empty;
        private string _qrImageSource = string.Empty;

        public ObservableCollection<string> ModeOfPayment { get; set; }

        public string SelectedModeOfPayment
        {
            get => _selectedModeOfPayment;
            set
            {
                if(_selectedModeOfPayment != value)
                {
                    _selectedModeOfPayment = value;
                    OnPropertyChanged();
                    UpdateQrCode();
                }
            }
        }

        public string QrImageSource
        {
            get => _qrImageSource;
            set
            {
                if(_qrImageSource != value)
                {
                    _qrImageSource = value;
                    OnPropertyChanged();
                }
            }
        }
        public PaymentInfoPageViewModel()
        {
            ModeOfPayment = new ObservableCollection<string>
            {
                "Gcash",
                "Paymaya"
            };
        }

        private void UpdateQrCode()
        {
            switch (SelectedModeOfPayment)
            {
                case "Gcash":
                    QrImageSource = "gcash.jpg";
                    break;

                case "Paymaya":
                    QrImageSource = "paymaya.jpg";
                    break;

                default:
                    QrImageSource = string.Empty;
                    break;
            }
        }
    }
}
