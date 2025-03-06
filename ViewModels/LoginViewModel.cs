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

namespace Barangay_Office.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _email;
        private string _password;
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


        public ICommand LoginCommand { get; }

        public LoginViewModel() : this(new AuthService()) { }
        public LoginViewModel(AuthService authService)
        {
            _authService = authService?? throw new ArgumentNullException(nameof(authService));
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private bool IsValid(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
        }


        private async Task LoginAsync()
        {
            if (!IsValid(Email))
            {
                Message = "Invalid Email format";
                TextColor = Colors.Red;
                return;
            }
            else { 
                var role = await _authService.LoginAsync(Email, Password);
                if(role != null)
                {
                    //navigate base on role
                    if (role == "Admin")
                    {
                        Message = "Admin Login Successful!";
                        TextColor = Colors.Green;
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else if(role == "Customer")
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
