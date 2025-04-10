using System.Text.RegularExpressions;
using System.Windows.Input;
using Barangay_Office.Services;
using SQLite;

namespace Barangay_Office.ViewModels
{
    internal class SignupPageViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        private string _email;
        private string _password;
        private string _confirmPasword;
        private string _message;
        private Color _BorderColor;
        private Color _emailColor = Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent;
        private Color _passwordColor = Application.Current?.Resources["BlueishPurple"] as Color?? Colors.Transparent;
        private Color _confirmPasswordColor = Application.Current?.Resources["BlueishPurple"] as Color?? Colors.Transparent;


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


        //colors
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                _BorderColor = value;
                OnPropertyChanged();
            }
        }

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


        //button command
        public ICommand RegisterCommand { get; }

        public SignupPageViewModel()
        {
            _email = string.Empty;
            _password = string.Empty;
            _confirmPasword = string.Empty;
            _message = string.Empty;
            _BorderColor = Colors.Transparent;
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

        private void ValidateAllFields()
        {

            bool isEmailEmpty = string.IsNullOrWhiteSpace(Email);
            bool isPasswordEmpty = string.IsNullOrWhiteSpace(Password);
            bool isConfirmPasswordEmpty = string.IsNullOrWhiteSpace(ConfirmPassword);


            //set messages
            Message = isEmailEmpty && isPasswordEmpty && isConfirmPasswordEmpty? "Fill all fields first."
                :isEmailEmpty? "Email Required."
                :isPasswordEmpty? "Password Required."
                :isConfirmPasswordEmpty? "Confirm Password Required."
                : string.Empty;

            //set Colors
            EmailColor = isEmailEmpty ? Colors.Red : (Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent);
            

            PasswordColor = isPasswordEmpty ? Colors.Red : (Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent);

            ConfirmPasswordColor = isConfirmPasswordEmpty ? Colors.Red : (Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent);

            //set BorderColor
            BorderColor = isEmailEmpty || isPasswordEmpty || isConfirmPasswordEmpty ? Colors.Red : (Application.Current?.Resources["BlueishPurple"] as Color ?? Colors.Transparent);

            //set color to default if there's a value
            if (!isEmailEmpty && Application.Current?.Resources["BlueishPurple"] is Color color) EmailColor = color;
            if (!isPasswordEmpty && Application.Current?.Resources["BlueishPurple"] is Color color2) PasswordColor = color2;
            if (!isConfirmPasswordEmpty && Application.Current?.Resources["BlueishPurple"] is Color color3) ConfirmPasswordColor = color3;

            return;
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

            //validate password format
            if (!IsValidPassword(Password))
            {
                Message = "Password must be at least 10-15 characters and contain atleast 1 letter and one number.";
                BorderColor = Colors.Red;
                PasswordColor = Colors.Red;
                return;
            }

            if(Password == ConfirmPassword)
            {

                //register new user if the pssword match
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
            else
            {
                ValidatePasswordMatch();
                return;
            }
        }
    }
}
