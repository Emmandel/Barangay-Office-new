using Barangay_Office.Models;
using Barangay_Office.Services;

namespace Barangay_Office.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;


    public LoginPage(AuthService authService)
    {
		InitializeComponent();
        _authService = authService;
    }

    private void OntogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ((ImageButton)sender).Source = PasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {

        string username = txtUsername.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Log in Error!", "Username Required", "OK");
            await Task.Delay(1000);
            return;
        }
        else if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Log in Error!", "Password Required", "OK");
            await Task.Delay(1000);
            return;

        }


        var role = await _authService.LoginAsync(username, password);
        if (!string.IsNullOrEmpty(role))
        {

            string WelcomeMessage = role == "Admin" ? "Welcome Admin!" : "Welcome Customer!";
            await DisplayAlert("Success", WelcomeMessage, "OK");

            string TargetPage = role == "Admin" ? nameof(MainPage) : nameof(CustomerPage);
            await Shell.Current.GoToAsync($"//{TargetPage}");

        }
        else
        {
            await DisplayAlert("Error", "Username or Password is Incorrect!", "OK");
        }

    }

    private async void LinkToSignUp(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(SignupPage)}");
    }

}