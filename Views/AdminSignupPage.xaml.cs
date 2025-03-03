using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Barangay_Office.Models;
using Barangay_Office.Services;
using SQLite;

namespace Barangay_Office.Views;

public partial class AdminSignupPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly SQLiteAsyncConnection _Connection;
	public AdminSignupPage(AuthService authService, SQLiteAsyncConnection db)
	{
		InitializeComponent();
        _authService = authService;
        _Connection = db;
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
    //validate and ignorecase
    private bool IsValid(string email)
    {
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
    }

    //signup button
    private async void AdminSignup_Clicked(object sender, EventArgs e)
    {
        //change to admin all
        string AdminEmail = txtAdminEmail.Text;
        string AdminPassword = AdminPasswordEntry.Text;
        string AdminConfirmPassword = ConfirmAdminPasswordEntry.Text;
        string role = "Admin";

        if (string.IsNullOrWhiteSpace(AdminEmail))
        {
            await DisplayAlert("Error!", "Email required", "OK");
            return;
        }

        //validate Admin Email
        if (!IsValid(AdminEmail))
        {
            await DisplayAlert("Error!", "Invalid Email format", "OK");
            return;
        }else if (string.IsNullOrWhiteSpace(AdminPassword))
        {
            await DisplayAlert("Error!", "Password required", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(AdminConfirmPassword))
        {
            await DisplayAlert("Error!", "Fill the confirm password", "OK");
            if(AdminPassword != AdminConfirmPassword)
            {
                await DisplayAlert("Error!", "Email required", "OK");
                return;
            }
            return;
        }

        var existingAdmin = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(a => a.Email == AdminEmail);
        if(existingAdmin != null)
        {
            await DisplayAlert("Error!", "Admin already exist!", "OK");
            return;
        }
        else
        {
            var newAdmin = new AdminUserInfo { Email = AdminEmail, Password = AdminPassword, Role = role };
            await _Connection.InsertAsync(newAdmin);

            await Task.Delay(500);
            await DisplayAlert("Success!", "Registered Successfully", "OK");
            await Navigation.PopAsync();
            //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

    }

    //link to login
    private async void LinkToLogin(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

}