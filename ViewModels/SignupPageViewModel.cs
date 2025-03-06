using System.Text.RegularExpressions;
using System.Windows.Input;
using Barangay_Office.Services;
using SQLite;

namespace Barangay_Office.ViewModels
{
    internal class SignupPageViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly SQLiteAsyncConnection _Connection;

        private string _email;
        private string _password;
        private string _confirmPasword;
        private string _message;
        private Color _BorderColor;
        private Color _passwordColor = (Color)Application.Current.Resources["BlueishPurple"];
        private Color _confirmPasswordColor = (Color)Application.Current.Resources["BlueishPurple"];


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

        public string ConfirmPassword
        {
            get => _confirmPasword;
            set
            {
                _confirmPasword = value;
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
            get => _BorderColor;
            set
            {
                _BorderColor = value;
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

        public ICommand RegisterCommand { get; }

        public SignupPageViewModel()
        {
            _authService = new AuthService();
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private void ValidatePasswordMatch()
        {
            if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                if (Password == ConfirmPassword)
                {
                    Message = "Passwords matched! galing mo diyan.";
                    BorderColor = Colors.Green;

                    PasswordColor = Colors.Green;
                    ConfirmPasswordColor = Colors.Green;
                }
                else
                {
                    Message = "Passwords do not match.";
                    BorderColor = Colors.Red;

                    PasswordColor = Colors.Red;
                    ConfirmPasswordColor = Colors.Red;
                }
            }
            else
            {
                PasswordColor = Colors.Red;
                ConfirmPasswordColor = Colors.Red;
            }
        }

        //validation accepts @gmail.com domain only
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }

        //validation accepts 10-15 characters, atleast 1 letter, 1 number and 1 special character
        private bool IsValidPassword(string password)
        {
            return password.Length >= 10 && password.Length <= 15 && Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,15}$");
        }

        public async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                Message = "Email required.";
                BorderColor = Colors.Red;
                return;
            }

            if (!IsValidEmail(Email))
            {
                Message = "Invalid Email format.";
                BorderColor = Colors.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                Message = "Password required.";
                BorderColor = Colors.Red;
                return;
            }

            if (!IsValidPassword(Password))
            {
                Message = "Password must be at least 10-15 characters and contain atleast 1 letter and one number.";
                BorderColor = Colors.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Message = "Confirm Password required.";
                BorderColor = Colors.Red;
                return;
            }

            if (Password != ConfirmPassword)
            {
                Message = "Passwords do not match.";
                BorderColor = Colors.Red;
                return;
            }

            bool success = await _authService.RegisterAsync(Email, Password, "Customer");

            if (success)
            {
                Message = "Registration successful!";
                await Shell.Current.GoToAsync("//LoginPage");
                BorderColor = Colors.Green;
            }
            else
            {
                Message = "User already exists!";
            }
        }
    }
}
