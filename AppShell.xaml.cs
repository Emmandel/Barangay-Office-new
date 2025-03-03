using Barangay_Office.Services;
using Barangay_Office.Views;

namespace Barangay_Office
{
    public partial class AppShell : Shell
    {
        private readonly AuthService _authService;
        public AppShell()
        {
            InitializeComponent();
            _authService = new AuthService();
            RegisteredRoutes();
            CheckAuthentication();

        }

        private async void CheckAuthentication()
        {
            try
            {

                // Check if user is authenticated
                var (isAuthenticated, role) = await _authService.GetAuthenticatedUserRoleAsync();

                if (!isAuthenticated)
                {
                    await Shell.Current.GoToAsync("//LoginPage"); //Redirect to login if not authenticated
                }
                else
                {
                    //Redirect based on role
                    if (role == "Admin")
                        await Shell.Current.GoToAsync("//MainPage"); //Admin goes to MainPage
                    else if (role == "Customer")
                        await Shell.Current.GoToAsync("//CustomerPage"); //Customer goes to CustomerPage
                    else
                        await Shell.Current.GoToAsync("//LoginPage"); //Default to login if role is invalid
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication check failed: {ex.Message}");
            }
        }

        private void RegisteredRoutes()
        {
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));//for adminpage
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(AdminProfile), typeof(AdminProfile));//for adminProfile
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));//Default Signuo Page
            //Routing.RegisterRoute(nameof(AdminSignupPage), typeof(AdminSignupPage));//Admin Signuo Page
            Routing.RegisterRoute(nameof(CustomerPage), typeof(CustomerPage));//for customer page
        }
    }
}
