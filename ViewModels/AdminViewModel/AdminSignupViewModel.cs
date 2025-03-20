using System.Text.RegularExpressions;
using SQLite;
using System.Windows.Input;
using Barangay_Office.Services;
using System.Xml.Serialization;

namespace Barangay_Office.ViewModels
{
    public class AdminSignupViewModel: BaseViewModel
    {
        private readonly AuthService _authService;

        private string _email;
        private string _password;
        private string _confirmPasword;
        private string _message;
        private Color _borderColor;
        private Color _emailColor = (Color)Application.Current.Resources["BlueishPurple"];
        private Color _passwordColor = (Color)Application.Current.Resources["BlueishPurple"];
        private Color _confirmPasswordColor = (Color)Application.Current.Resources["BlueishPurple"];

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

        public string ConfirmPassword
        {
            get => _confirmPasword;
            set
            {
                _confirmPasword = value;
                ValidateAllFields();
                ValidatePasswordMatch();
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

        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                OnPropertyChanged();
            }
        }

        //colors
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

        public Color ConfirmPasswordColor
        {
            get => _confirmPasswordColor;
            set
            {
                _confirmPasswordColor = value;
                OnPropertyChanged();
            }
        }



        public ICommand AdminRegisterCommand { get; }

        public AdminSignupViewModel() : this(new AuthService()) { }

        public AdminSignupViewModel(AuthService authService)
        {
            _authService = new AuthService();
            AdminRegisterCommand = new Command(async () => await RegisterAsync());
            _authService = authService;
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 10 && password.Length <= 15 && Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,15}$");
        }

        private void ValidateAllFields()
        {
            bool isEmailEmpty = string.IsNullOrWhiteSpace(Email);
            bool isPasswordEmpty = string.IsNullOrWhiteSpace(Password);
            bool isConfirmPasswordEmpty = string.IsNullOrWhiteSpace(ConfirmPassword);

            //set messages
            Message = isEmailEmpty && isPasswordEmpty && isConfirmPasswordEmpty ? "Fill all the fields first."
                : isEmailEmpty ? "Email Required."
                : isPasswordEmpty ? "Password Required."
                : isConfirmPasswordEmpty ? "Confirm Password Required."
                : string.Empty;

            //set the colors
            EmailColor = isEmailEmpty ? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            PasswordColor = isPasswordEmpty? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];
            
            ConfirmPasswordColor = isConfirmPasswordEmpty? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            //set border color
            BorderColor = isEmailEmpty || isPasswordEmpty || isConfirmPasswordEmpty? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            //set color to default if there's a value
            if(!isEmailEmpty) EmailColor = (Color)Application.Current.Resources["BlueishPurple"];
            if(!isPasswordEmpty) PasswordColor = (Color)Application.Current.Resources["BlueishPurple"];
            if(!isConfirmPasswordEmpty) ConfirmPasswordColor = (Color)Application.Current.Resources["BlueishPurple"];

            return;
        }

        private void ValidatePasswordMatch()
        {
            if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                if (Password == ConfirmPassword)
                {
                    Message = "Passwords matched! galing mo diyan.";
                    BorderColor = Colors.Green;

                    PasswordColor = BorderColor;
                    ConfirmPasswordColor = BorderColor;
                }
                else
                {
                    Message = "Passwords do not match.";
                    BorderColor = Colors.Red;

                    PasswordColor = BorderColor;
                    ConfirmPasswordColor = BorderColor;
                }
            }
            else
            {
                PasswordColor = Colors.Red;
                ConfirmPasswordColor = Colors.Red;
            }
            return;
        }

        public async Task RegisterAsync()
        {
            ValidateAllFields();
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                return;
            }

            //validate email format
            if (!IsValidEmail(Email))
            {
                Message = "Invalid Email format.";
                EmailColor = Colors.Red;
                BorderColor = Colors.Red;
                return;
            }

            //validate password
            if (!IsValidPassword(Password))
            {
                Message = "Password must be at least 10-15 characters and contain atleast 1 letter and one number.";
                BorderColor = Colors.Red;
                PasswordColor = Colors.Red;
                return;
            }


            if (Password == ConfirmPassword)
            {
                //register new admin
                bool success = await _authService.RegisterAsync(Email, Password, "Admin");
                if (success)
                {
                    Message = "Registration successful!";
                    await Shell.Current.Navigation.PopAsync();
                    BorderColor = Colors.Green;
                }
                else
                {
                    Message = "User already exists!";
                }
            }
            else
            {
                ValidatePasswordMatch();
                return;
            }
        }
    }
}
