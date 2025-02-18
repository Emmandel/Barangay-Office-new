using Barangay_Office.Models;
using Barangay_Office.Services;

namespace Barangay_Office.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;

    private readonly LocalDatabase _DbService;

    public LoginPage(AuthService authService)
    {
		InitializeComponent();
        _authService = authService;
        _DbService = new LocalDatabase();
    }

    private void OntogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ((ImageButton)sender).Source = PasswordEntry.IsPassword ? "open_eye.png" : "close_eye.png";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string email = txtUsername.Text;
        string pass = PasswordEntry.Text;

        var user = await _DbService.AuthenticateUser(email, pass);

        if (user != null)
        {
            //user is authenticated and store their role
            _authService.Login(user.Role);
            if (user.Role == "super_admin")
            {
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
            }
        }
        else
        {
            await DisplayAlert("Login Failed!", " Invalid username or password","OK");
        }
    }

}