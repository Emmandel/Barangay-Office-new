using Barangay_Office.Services;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace Barangay_Office.Views;

public partial class LoadingPage : ContentPage
{
    private readonly AuthService _authService;

    public LoadingPage(AuthService authService)
    {
		InitializeComponent();
        _authService = authService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await NavigateAsync();
        

    }

    private async Task NavigateAsync()
    {
        try
        {

            if (await _authService.IsAuthenticatedAsync())
            {
                string userRole = _authService.GetRole();
                if(userRole == "Admin"){
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }else if(userRole == "Customer")
                {
                    await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
            else
            {
                //user has not logged in
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred:{ex.Message}", "OK");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}