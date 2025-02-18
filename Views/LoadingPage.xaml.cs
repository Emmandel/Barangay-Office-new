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

        bool isAuthenticated = await _authService.IsAuthenticatedAsync();
        string role = _authService.GetUserRole();

        if (isAuthenticated)
        {
            //redirect based on the user's role
            if (role == "super_admin")
            {
                //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(CustomerPage)}");
            }
        }
        else
        {
            //user has not logged in
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}