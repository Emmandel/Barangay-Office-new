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
        ((ImageButton)sender).Source = PasswordEntry.IsPassword ? "open_eye.png" : "close_eye.png";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        _authService.LogIn();
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

}