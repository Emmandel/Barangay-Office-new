using Barangay_Office.Services;
using Barangay_Office.Views.SuperAdmin;

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
        try 
        {
            await NavigateAsync();
        }
        catch (Exception ex)
        {
            // Ensure app doesn't crash
            await DisplayAlert("Error", $"Application error: {ex.Message}", "OK");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        
    }

    private async Task NavigateAsync()
    {
        try
        {
            await Task.Delay(2000);
            
            var authState = await _authService.IsAuthenticatedAsync();
            if (!authState)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }
            string userRole = AuthService.GetRole();
            string targetPage = userRole switch
            {
                "SuperAdmin" => nameof(SuperAdminPage),
                "Admin" => nameof(MainPage),
                "Customer" => nameof(CustomerPage),
                _ => nameof(LoginPage)
            };
            await Shell.Current.GoToAsync($"//{targetPage}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Navigation error: {ex.Message}", "OK");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }

    //add memory management
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

}