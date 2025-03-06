using System.Threading.Tasks;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.ViewModels;

namespace Barangay_Office.Views;

public partial class LoginPage : ContentPage
{


    public LoginPage(LoginViewModel viewModel)
    {
		InitializeComponent();
        BindingContext = viewModel;
    }

    private async void LinkToSignup(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(SignupPage)}");
    }

    private void OntogglePasswordVisibility(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        ((ImageButton)sender).Source = PasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }

    private async void LinkToForgotPassword(object sender, TappedEventArgs e) => await Navigation.PushAsync(new ForgotPasswordPage(new AuthService()));//redirect to reset password page
}