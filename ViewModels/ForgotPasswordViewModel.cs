using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Barangay_Office.Services;

namespace Barangay_Office.ViewModels
{
    public class ForgotPasswordViewModel: BaseViewModel
    {
        private readonly AuthService _authService;
        private string _email;
        private string _newPassword;
        private string _message;
        private Color _borderColor = (Color)Application.Current.Resources["BlueishPurple"];
        private Color _emailColor = (Color)Application.Current.Resources["BlueishPurple"];
        private Color _resetPasswordColor = (Color)Application.Current.Resources["BlueishPurple"];


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

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
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
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
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

        public Color NewPassColor
        {
            get => _resetPasswordColor;
            set
            {
                _resetPasswordColor = value;
                OnPropertyChanged();
            }
        }

        public ICommand ResetPasswordCommand { get; }
        public ForgotPasswordViewModel() : this(new AuthService()) { }

        public ForgotPasswordViewModel(AuthService authService)
        {
            _authService = authService;
            ResetPasswordCommand = new Command(async () => await ResetPasswordAsync());
        }

        //validate fields
        private void ValidateAllFields()
        {
            bool isEmailEmpty = string.IsNullOrWhiteSpace(Email);
            bool isNewPasswordEmpty = string.IsNullOrWhiteSpace(NewPassword);

            //set messages
            Message = isEmailEmpty && isNewPasswordEmpty ? "Fill all fields first."
                : isEmailEmpty? "Email Required."
                : isNewPasswordEmpty? "Enter your new password."
                : string.Empty;

            //set colors
            EmailColor = isEmailEmpty? Colors.Red : (Color)Application.Current.Resources["BlueishPurple"];

            NewPassColor = isNewPasswordEmpty? Colors.Red: (Color)Application.Current.Resources["BlueishPurple"];

            //check if any of them have error
            BorderColor = isEmailEmpty || isNewPasswordEmpty? Colors.Red: (Color)Application.Current.Resources["BlueishPurple"];

            //set color to default
            if(!isEmailEmpty) EmailColor = (Color)Application.Current.Resources["BlueishPurple"];
            if(!isNewPasswordEmpty) NewPassColor = (Color)Application.Current.Resources["BlueishPurple"];
            return;
        }


        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }

        private bool IsValidPassword(string newpassword)
        {
            return newpassword.Length >= 10 && newpassword.Length <= 15 && Regex.IsMatch(newpassword, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,15}$");
        }

        //method for resetting the password
        public async Task ResetPasswordAsync()
        {
            ValidateAllFields();

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(NewPassword))
            {
                Message = "All fields are required!";
                BorderColor = Colors.Red;
                return;
            }

            //validate format
            if (!IsValidEmail(Email))
            {
                Message = "Invalid Email format.";
                EmailColor = Colors.Red;
                BorderColor = Colors.Red;
                return;
            }

            if (!IsValidPassword(NewPassword))
            {
                Message = "Password have least 10-15 alphanumeric";
                BorderColor = Colors.Red;
                NewPassColor = Colors.Red;
                return;
            }


            bool isEmailRegistered = await _authService.IsEmailRegisteredAsync(Email);

            if (!isEmailRegistered)
            {
                Message = "Email is not registered!";
                BorderColor = Colors.Red;
                return;
            }

            bool isReset = await _authService.ResetPasswordAsync(Email, NewPassword);

            if (isReset)
            {
                Message = "Password has been reset successfully!";
                BorderColor = Colors.Green;

                await Task.Delay(500);
                await Shell.Current.Navigation.PopAsync();  // Navigate back to login

                return;
            }
            else
            {
                Message = "Something went wrong!";
                BorderColor = Colors.Red;
            }
        }

    }
}
