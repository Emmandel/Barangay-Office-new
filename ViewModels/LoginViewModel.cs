using System.Text.RegularExpressions;
using System.Windows.Input;
using Barangay_Office.Services;
using Barangay_Office.Views;
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
        private Color _emailColor = Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent;
        private Color _passwordColor = Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent;
        private bool _isBiometricEnabled;

        //entry accessors
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

        //color accessors
        public Color EmailColor
        {
            get => _emailColor;
            set
            {
                _emailColor = value;
                OnPropertyChanged();
            }
        }

        public Color PasswordColor
        {
            get => _passwordColor;
            set
            {
                _passwordColor = value;
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
            _email = string.Empty;
            _password = string.Empty;
            _message = string.Empty;
            _borderColor = Colors.Transparent;
            _authService = authService?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new Command(async () => await LoginAsync());
            BiometricLoginCommand = new Command(async () => await BiometricLoginAsync());

            //enable biometrics exlusively for customer
            CheckBiometricEligibility();
        }

        //check if it's a customer
        public void CheckBiometricEligibility()
        {

            // Retrieve the current user role
            string currentUserRole = Preferences.Get("UserRole", string.Empty);

            if (string.IsNullOrEmpty(currentUserRole))
            {
                // If no user is logged in, check the last user role for biometric eligibility
                string lastUserRole = Preferences.Get("LastUserRole", string.Empty);
                IsBiometricEnabled = lastUserRole == "Customer" || lastUserRole == "Admin";
            }
            else
            {
                // If a user is logged in, enable biometrics based on their current role
                IsBiometricEnabled = currentUserRole == "Customer" || currentUserRole == "Admin";
            }
        }

        //validation for email
        private static bool IsValid(string email)
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
            EmailColor = isEmailEmpty? Colors.Red : Application.Current?.Resources["BlueishPurple"] as Color?? Colors.Transparent;

            PasswordColor = isPasswordEmpty ? Colors.Red : Application.Current?.Resources["BlueishPurple"] as Color?? Colors.Transparent;

            //set BorderColor
            BorderColor = isEmailEmpty || isPasswordEmpty ? Colors.Red : Application.Current?.Resources["BlueishPurple"] as Color?? Colors.Transparent;

            //set color to default if there's a value
            if (!isEmailEmpty && Application.Current?.Resources["BlueishPurple"] is Color color) EmailColor = color;
            if (!isPasswordEmpty && Application.Current?.Resources["BlueishPurple"] is Color color2) PasswordColor = color2;
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
                    else if (role == "Customer")
                    {
                        Message = "Customer Login Successful!";
                        BorderColor = Colors.Green;
                        await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
                    }
                    else
                    {
                        //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                        Message = "Unexpected role!";
                        BorderColor = Colors.Red;
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
                    Message = "Biometric Authentication is not available!";
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
