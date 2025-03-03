using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Google.Apis.Admin.Directory.directory_v1.Data;
using SQLite;

namespace Barangay_Office.Views;

public partial class SignupPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly SQLiteAsyncConnection _Connection;



    public SignupPage(AuthService authService, SQLiteAsyncConnection db)
    {
        InitializeComponent();
        _authService = authService;
        _Connection = db;

        txtEmail.Keyboard = Keyboard.Email;

    }

    //simple validation for email
    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$", RegexOptions.IgnoreCase);
    }

    private async void Signup_Clicked(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;
        string role = "Customer";
        //string role = RolePicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Error", "Email required", "OK");
            return;
        }

        //validate email before proceeding
        if (!IsValidEmail(email))
        {
            await DisplayAlert("Error", "Invalid Email format", "OK");
            return;
        }

        else if (string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Password required", "OK");
            return;
        }
        
        if(string.IsNullOrWhiteSpace(confirmPassword))
        {

            await DisplayAlert("Error", "Fill the Confirm Password!", "OK");
            return;
        }
        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Password don't match", "OK");
            return;
        }        

        var existingUser = await _Connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            await DisplayAlert("Error", "User Already Exist!", "OK");
            return;
        }

        var newUser = new AdminUserInfo { Email = email, Password = password, Role = role }; //store all inputs at the database
        await _Connection.InsertAsync(newUser); //insert into kapag sa mysql

        await Task.Delay(1000);
        await DisplayAlert("Success", "Registered Successfully", "OK");
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

    private void OntogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ((ImageButton)sender).Source = PasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }

    private async void LinkToLogin(object sender, TappedEventArgs e)
    {
		await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

    private void OntoggleConfirmPasswordVisibility(object sender, EventArgs e)
    {
        ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
        ((ImageButton)sender).Source = ConfirmPasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }
}