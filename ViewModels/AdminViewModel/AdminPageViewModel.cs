using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Utilities;

namespace Barangay_Office.ViewModels
{
    public class AdminPageViewModel : BaseViewModel
    {
        public ICommand AdminNavigate { get; }
        public ICommand ToReset { get; }

        public AdminPageViewModel()
        {


            AdminNavigate = new RelayCommand(async swe =>
            {
                if (swe is string pages && !string.IsNullOrWhiteSpace(pages))
                {
                    await Shell.Current.GoToAsync($"///{pages}");
                }
            });
        }
    }
}
