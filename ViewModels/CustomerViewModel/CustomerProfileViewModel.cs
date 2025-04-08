using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Services;
using Barangay_Office.Utilities;
using Barangay_Office.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Barangay_Office.ViewModels.CustomerViewModel
{
    public class CustomerProfileViewModel : BaseViewModel
    {
        private AuthService _authService;
        public ICommand ToReset { get; }
        public ICommand ToLogout { get; }

        public CustomerProfileViewModel() : this(new AuthService()) { }
        public CustomerProfileViewModel(AuthService authService)
        {
            _authService = authService;

            ToReset = new RelayCommand(async tr =>
            {
                await Shell.Current.Navigation.PushAsync(new ForgotPasswordPage(new AuthService()));
            });

            ToLogout = new RelayCommand(async tl =>
            {
                _authService.LogOut();

                var loginViewModel = new LoginViewModel(_authService);
                loginViewModel.CheckBiometricEligibility();
                await Task.Delay(500);

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });
        }
    }
}
