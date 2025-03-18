using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Services;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class SuperAdminProfileViewModel : BaseViewModel
    {
        public ICommand ToReset { get; }
        public ICommand ToAdminSignup { get; }

        public ICommand ToLogout { get; }
        
        public SuperAdminProfileViewModel()
        {
            ToAdminSignup = new RelayCommand(async (tas) =>
            {
                await Shell.Current.Navigation.PushAsync(new AdminSignupPage(new AuthService()));
            });
            ToReset = new RelayCommand(async (tr) =>
            {
                await Shell.Current.Navigation.PushAsync(new ForgotPasswordPage(new AuthService()));
            });

            ToLogout = new RelayCommand(async (TLO) =>
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });
        }
    }
}
