using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Barangay_Office.ViewModels
{
    public partial class LoginPageViewModel : BaseModel
    {
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        public async void Login()
        {

        }
    }
}
