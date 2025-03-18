
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class SuperAdminPageViewModels : BaseViewModel
    {
        public ICommand SuperAdminNavigate { get; }
        

        public SuperAdminPageViewModels()
        {
            SuperAdminNavigate = new RelayCommand(async sn =>
            {
                if(sn is string pages && !string.IsNullOrWhiteSpace(pages))
                {
                    await Shell.Current.GoToAsync($"///{pages}");
                }
            });
        }
    }
}
