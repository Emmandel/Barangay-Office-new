using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels.CustomerViewModel
{
    public class C_AboutUsPageViewModel : BaseViewModel
    {
        public ICommand OnCustomerServiceClicked { get; }

        public C_AboutUsPageViewModel()
        {
            OnCustomerServiceClicked = new RelayCommand(async OC =>
            {
                var mainPage = Application.Current?.Windows[0]?.Page;
                if (mainPage != null)
                {
                    await Shell.Current.GoToAsync($"//{nameof(ChatPage)}");
                }
            });
        }
    }
}
