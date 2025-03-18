using Barangay_Office.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barangay_Office.ViewModels
{
    public class SuperAdminProfileViewModel : BaseViewModel
    {
        //Commands
        public ICommand ToSuperAdminSignup { get; }
        public ICommand ToLogout { get; }

        private readonly AuthService _authService;
        public SuperAdminProfileViewModel(AuthService authService) { 
            _authService = authService;

            _authService = authService;
            //go to admin signup
            ToAdminSignup = new Command(async () =>
            {
                _authService.LogOut();
                await Application.Current.MainPage.Navigation.PushAsync(new AdminSignupPage(_authService));
            });

            //go to logout
            ToLogout = new Command(() =>
            {
                _authService.LogOut();
                Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });
        }
    }
}
