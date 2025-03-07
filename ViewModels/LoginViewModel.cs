using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Services;
using Barangay_Office.Views;
using MongoDB.Driver.Core.Misc;
using Plugin.Maui.Biometric;

namespace Barangay_Office.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _email;
        private string _password;
        private string _message;
        private Color _textColor;
        private bool _isBiometricEnabled;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                OnPropertyChanged();
            }
        }
        public bool IsBiometricEnabled { 
            get => _isBiometricEnabled;
            set
            {
                _isBiometricEnabled = value;
                OnPropertyChanged();
            }
        }


        public ICommand LoginCommand { get; }
        public ICommand BiometricLoginCommand { get; }



        public LoginViewModel() : this(new AuthService()) { }
        public LoginViewModel(AuthService authService)
        {
            _authService = authService?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new Command(async () => await LoginAsync());
            BiometricLoginCommand = new Command(async () => await BiometricLoginAsync());

            //enable biometrics exlusively for customer
            CheckBiometricEligibility();
        }

        //check if it's a customer
        public void CheckBiometricEligibility()
        {
            //Retrieve the last logged-in user's role
            string userRole = Preferences.Get("UserRole", Preferences.Get("LastUserRole", string.Empty));
            IsBiometricEnabled = userRole == "Customer";
        }

        //validation for email
        private bool IsValid(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }


        //login using biometric for customer only
        private async Task BiometricLoginAsync()
        {
            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
            {
                Title = "Authenticate to login",
                NegativeText = "Cancel"
            }, CancellationToken.None);


            if (result.Status == BiometricResponseStatus.Success)
            {
                //retrieve the stored role
                string userRole = Preferences.Get("UserRole",Preferences.Get("LastUserRole",string.Empty));

                if(userRole == "Customer")
                {
                    await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
                }
                else
                {
                    Message = "Biometric Authentication is available only for Customers!";
                    TextColor = Colors.Red;
                    return;
                }
            }
            else
            {
                Message = "Biometric Authentication Failed!";
                TextColor = Colors.Red;
                return;
            }
        }

        //manual logging in
        private async Task LoginAsync()
        {

            if (!IsValid(Email))
            {
                Message = "Invalid Email format";
                TextColor = Colors.Red;
                return;
            }
            else
            {
                var role = await _authService.LoginAsync(Email, Password);
                if (role != null)
                {
                    Preferences.Set("UserRole", role);

                    //update Biometric Visibility
                    CheckBiometricEligibility();

                    //navigate base on role
                    if (role == "Admin")
                    {
                        Message = "Admin Login Successful!";
                        TextColor = Colors.Green;
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else if (role == "Customer")
                    {
                        Message = "Customer Login Successful!";
                        TextColor = Colors.Green;
                        await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                else
                {
                    Message = "Invalid Username or Password. Please try again!";
                    TextColor = Colors.Red;
                    return;
                }
            }
        }
    }
}
