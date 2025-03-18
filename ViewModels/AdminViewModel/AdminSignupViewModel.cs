using System.Text.RegularExpressions;
using SQLite;
using System.Windows.Input;
using Barangay_Office.Services;

namespace Barangay_Office.ViewModels
{
    public class AdminSignupViewModel: BaseViewModel
    {
        private readonly AuthService _authService;

        private string _email;
        private string _password;
        private string _confirmPasword;
        private string _message;
        private Color _textColor;

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

        public async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                Message = "Email required.";
                TextColor = Colors.Red;
                return;
            }

            if (!IsValidEmail(Email))
            {
                Message = "Invalid Email format.";
                TextColor = Colors.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                Message = "Password required.";
                TextColor = Colors.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Message = "Confirm Password required.";
                TextColor = Colors.Orange;
                return;
            }

            if (Password != ConfirmPassword)
            {
                Message = "Passwords do not match.";
                TextColor = Colors.Red;
                return;
            }

            bool success = await _authService.RegisterAsync(Email, Password, "Admin");

            if (success)
            {
                Message = "User created successfully!";
                TextColor = Colors.Green;
                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                Message = "User already exists!";
            }
        }
    }
}
