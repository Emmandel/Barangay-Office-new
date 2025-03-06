using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
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
        public ICommand ResetPasswordCommand { get; }
        public ForgotPasswordViewModel() : this(new AuthService()) { }

        public ForgotPasswordViewModel(AuthService authService)
        {
            _authService = authService;
            ResetPasswordCommand = new Command(async () => await ResetPasswordAsync());
        }

        //method for resetting the password
        public async Task ResetPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(NewPassword))
            {
                Message = "All fields are required!";
                TextColor = Colors.Red;
                return;
            }

            bool isEmailRegistered = await _authService.IsEmailRegisteredAsync(Email);

            if (!isEmailRegistered)
            {
                Message = "Email is not registered!";
                TextColor = Colors.Red;
                return;
            }

            bool isReset = await _authService.ResetPasswordAsync(Email, NewPassword);

            if (isReset)
            {
                Message = "Password has been reset successfully!";
                //await Navigation.PopAsync(); // Navigate back to login
                TextColor = Colors.Green;
                return;
            }
            else
            {
                Message = "Something went wrong!";
                TextColor = Colors.Red;
            }
        }

    }
}
