using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using Google.Apis.Admin.Directory.directory_v1.Data;
using SQLite;

namespace Barangay_Office.Views;

public partial class SignupPage : ContentPage
{
    public SignupPage()
    {
        InitializeComponent();
        BindingContext = new SignupPageViewModel();

    }
    private void OntogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ((ImageButton)sender).Source = PasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }

    private void OntoggleConfirmPasswordVisibility(object sender, EventArgs e)
    {
        ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
        ((ImageButton)sender).Source = ConfirmPasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }
    private async void LinkToLogin(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}