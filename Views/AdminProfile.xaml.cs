using Barangay_Office.Services;
using SQLite;

namespace Barangay_Office.Views;

public partial class AdminProfile : ContentPage
{
	private readonly AuthService _authService;
    public AdminProfile(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
    }

    private void LogOutButton_Clicked(object sender, EventArgs e)
    {
        //the user will be directed to the login page
        _authService.LogOut();
        Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

    private async void AdminSignup_Clicked(object sender, EventArgs e) {

        _authService.LogOut();
        await Navigation.PushAsync(new AdminSignupPage(_authService));
    }
    
}