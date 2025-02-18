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
        if (await _authService.IsAuthenticatedAsync())
        {
            //user is logged in
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            //user need to log in
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}