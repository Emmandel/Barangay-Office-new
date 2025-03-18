using System.Text.RegularExpressions;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using SQLite;

namespace Barangay_Office.Views;

public partial class AdminSignupPage : ContentPage
{
	public AdminSignupPage(AuthService authService)
	{
		InitializeComponent();
        BindingContext = new AdminSignupViewModel(authService);
	}

    private void OntogglePasswordVisibility(object sender, EventArgs e)
    {
        AdminPasswordEntry.IsPassword = !AdminPasswordEntry.IsPassword;
        ((ImageButton)sender).Source = AdminPasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }

    private void OntoggleConfirmPasswordVisibility(object sender, EventArgs e)
    {
        ConfirmAdminPasswordEntry.IsPassword = !ConfirmAdminPasswordEntry.IsPassword;
        ((ImageButton)sender).Source = ConfirmAdminPasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";

    }


}