using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Services;
using Barangay_Office.Utilities;
using Barangay_Office.Views;
using Barangay_Office.Views.Admin;

namespace Barangay_Office.ViewModels
{
    public class SuperAdminProfileViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        public ICommand ToReset { get; }
        public ICommand ToAdminSignup { get; }

        public ICommand ToLogout { get; }

        public SuperAdminProfileViewModel() : this(new AuthService()) { }//assign this as a default constructor

        public SuperAdminProfileViewModel(AuthService authService)
        {
            _authService = authService;
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
                _authService.LogOut();// do not forget this when you want to logout
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });
        }
    }
}
