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

namespace Barangay_Office.ViewModels
{
    public class CustomerProfileViewModel : BaseViewModel
    {
        private AuthService _authService;

        private ICommand SelectLanguage { get; }
        public ICommand ToReset { get; }
        public ICommand ToLogout { get; }
        public ICommand ToggleTheme { get; }
        

        public CustomerProfileViewModel() : this(new AuthService()) { }

        public CustomerProfileViewModel(AuthService authService)
        {
            _authService = authService;

            SelectLanguage = new RelayCommand(_ =>
            {
                return Task.CompletedTask;
            });

            ToReset = new RelayCommand(async tr =>
            {
                await Shell.Current.Navigation.PushAsync(new ForgotPasswordPage(_authService));
            });

            ToLogout = new RelayCommand(async tl =>
            {
                _authService.LogOut();

                var loginViewModel = new LoginViewModel(_authService);
                loginViewModel.CheckBiometricEligibility();

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });

            ToggleTheme = new RelayCommand(_ =>
            {
                if (Application.Current?.UserAppTheme == AppTheme.Light)
                {
                    SetTheme(AppTheme.Dark);
                }
                else
                {
                    SetTheme(AppTheme.Light);
                }
                return Task.CompletedTask;
            });
        }

        //toggle theme
        private void SetTheme(AppTheme theme)
        {
            if (Application.Current != null)
            {
                Application.Current.UserAppTheme = theme;
            }
        }
    }
}
