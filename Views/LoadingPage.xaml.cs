using Barangay_Office.Services;

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
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else
        {
            //user has not logged in
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}