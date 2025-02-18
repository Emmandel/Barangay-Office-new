using Barangay_Office.Services;

namespace Barangay_Office.Views;

public partial class ProfilePage : ContentPage
{
    private readonly AuthService _authService;

    public ProfilePage(AuthService authService)
    {
		InitializeComponent();
        _authService = authService;
    }

    private void OutButton_Clicked(object sender, EventArgs e)
    {
        _authService.Logout();
        Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}