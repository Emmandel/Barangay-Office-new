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
        private Color _borderColor;
        private Color _emailColor = (Color)Application.Current.Resources["BlueishPurple"];
        private Color _passwordColor = (Color)Application.Current.Resources["BlueishPurple"];
        private bool _isBiometricEnabled;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                ValidateAllFields();
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                ValidateAllFields();
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

        //colors
        public Color EmailColor
        {
            get => _passwordColor;
            set
            {
                _passwordColor = value;
                OnPropertyChanged();
            }
        }

        public Color PasswordColor
        {
            get => _emailColor;
            set
            {
                _emailColor = value;
                OnPropertyChanged();
            }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                OnPropertyChanged();
            }
        }
        

        //biometric
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
            IsBiometricEnabled = userRole == "Admin";
            IsBiometricEnabled = userRole == "SuperAdmin";
        }

        //validation for email
        private bool IsValid(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }

        //validate all fields
        private void ValidateAllFields()
        {
            bool isEmailEmpty = string.IsNullOrWhiteSpace(Email);
            bool isPasswordEmpty = string.IsNullOrWhiteSpace(Password);

            //set Messages
            Message = isEmailEmpty && isPasswordEmpty ? "Fill all the fields first."
                : isEmailEmpty? "Enter your email first"
                : isPasswordEmpty? "Enter your password next"
                : string.Empty;



            //set Colors
            EmailColor = isEmailEmpty? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            PasswordColor = isPasswordEmpty ? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            //set BorderColor
            BorderColor = isEmailEmpty || isPasswordEmpty ? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            //set color to default if there's a value
            if (!isEmailEmpty) EmailColor = (Color)Application.Current.Resources["BlueishPurple"];
            if (!isPasswordEmpty) PasswordColor = (Color)Application.Current.Resources["BlueishPurple"];
        }

        //manual logging in
        private async Task LoginAsync()
        {

            ValidateAllFields();
            // Check if email or password is empty
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            if (!IsValid(Email))
            {
                Message = "Invalid Email Format.";
                BorderColor = Colors.Red;
                EmailColor = Colors.Red;
                return;
            }
            else {
                var role = await _authService.LoginAsync(Email, Password);
                if (role != null)
                {
                    Preferences.Set("UserRole", role);

                    //update Biometric Visibility
                    CheckBiometricEligibility();

                    //navigate base on role
                    if (role == "SuperAdmin")
                    {
                        Message = "Super Admin Login Successful!";
                        await Shell.Current.GoToAsync($"//{nameof(SuperAdminPage)}");
                        BorderColor = Colors.Green;
                    }
                    else if (role == "Admin")
                    {
                        Message = "Admin Login Successful!";
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                        BorderColor = Colors.Green;
                    }
                    else if (role == "SuperAdmin")
                    {
                        Message = "Admin Login Successful!";
                        await Shell.Current.GoToAsync($"//{nameof(SuperAdminPage)}");
                        BorderColor = Colors.Green;
                    }
                    else if (role == "Customer")
                    {
                        Message = "Customer Login Successful!";
                        BorderColor = Colors.Green;
                        await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                else
                {
                    Message = "Invalid, Email or Password does not exists.";
                    BorderColor = Colors.Red;
                    EmailColor = Colors.Red;
                    PasswordColor = Colors.Red;
                    return;
                } 
            }
            
        }

        //login using biometric for customer only
        private async Task BiometricLoginAsync()
        {
            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
            {
                Title = "Authenticate to login",
                NegativeText = "Cancel"
            }, CancellationToken.None);



            //prompt the user
            if (result.Status == BiometricResponseStatus.Success)
            {
                //retrieve the stored role
                string userRole = Preferences.Get("UserRole",Preferences.Get("LastUserRole",string.Empty));

                if(userRole == "Customer")
                {
                    await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
                }
                else if(userRole == "Admin")
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else if (userRole == "SuperAdmin")
                {
                    await Shell.Current.GoToAsync($"//{nameof(SuperAdminPage)}");
                }
                else
                {
                    Message = "Biometric Authentication is available only for Customers!";
                    BorderColor = Colors.Red;
                    return;
                }
            }
            else
            {
                Message = "Biometric Authentication Failed!";
                BorderColor = Colors.Red;
                return;
            }
        }
    }
}
