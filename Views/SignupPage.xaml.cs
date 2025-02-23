using System.Threading.Tasks;
using Barangay_Office.Models;
using Barangay_Office.Services;
using SQLite;

namespace Barangay_Office.Views;

public partial class SignupPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly SQLiteAsyncConnection _connection;



    public SignupPage(AuthService authService, SQLiteAsyncConnection db)
    {
        InitializeComponent();
        _authService = authService;
        _connection = db;

    }
    private async void Signup_Clicked(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;
        string role = RolePicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Error", "Username required", "OK");
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

        if (string.IsNullOrEmpty(role))
        {
            await DisplayAlert("Error", "Enter your role first!", "OK");
            return;

        }
        

        var existingUser = await _connection.Table<AdminUserInfo>().FirstOrDefaultAsync(u => u.Username == username);
        if (existingUser != null)
        {
            await DisplayAlert("Error", "User Already Exist!", "OK");
            return;
        }


        var newUser = new AdminUserInfo { Username = username, Password = password, Role = role }; //store all inputs at the database
        await _connection.InsertAsync(newUser); //insert into kapag sa mysql

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