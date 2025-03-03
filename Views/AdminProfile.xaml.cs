using Barangay_Office.Services;

namespace Barangay_Office.Views;

public partial class AdminProfile : ContentPage
{
	

    private void LogOutButton_Clicked(object sender, EventArgs e)
    {
        //the user will be directed to the login page
        _authService.LogOut();
        Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}