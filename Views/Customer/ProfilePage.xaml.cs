using System.Threading.Tasks;
using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using Barangay_Office.ViewModels.CustomerViewModel;

namespace Barangay_Office.Views;

public partial class ProfilePage : ContentPage
{
    //private readonly AuthService _authService;

    public ProfilePage()
    {
		InitializeComponent();
        //_authService = authService;
        BindingContext = new CustomerProfileViewModel();
    }

    //private async void OutButton_Clicked(object sender, EventArgs e)
    //{

    //    //the user will be directed to the login page
    //    _authService.LogOut();

    //    var loginViewModel = new LoginViewModel(_authService);
    //    loginViewModel.CheckBiometricEligibility();
    //    await Task.Delay(500);
    //    Shell.Current.GoToAsync($"//{nameof(LoginPage)}");

    //}

    //private async void ToReset(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new ForgotPasswordPage(new AuthService()));
    //}
}