using Barangay_Office.Services;
using Barangay_Office.ViewModels;
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
            InitializeStartupNavigation();

        }

        private async void InitializeStartupNavigation()
        {
            await GoToAsync($"//{nameof(LoadingPage)}");
        }

        private async void CheckAuthentication()
        {
            try
            {
                // Check if user is authenticated
                var (isAuthenticated, Role) = await _authService.GetAuthenticatedUserRoleAsync();

                if (!isAuthenticated)
                {
                    await Shell.Current.GoToAsync("//LoginPage"); //Redirect to login if not authenticated
                }
                else
                {
                    //Redirect based on role
                    if (Role == "SuperAdmin")
                        await Shell.Current.GoToAsync("//SuperAdminPage");
                    else if (Role == "Admin")
                        await Shell.Current.GoToAsync("//MainPage"); //Admin goes to MainPage
                    else if (Role == "Customer")
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
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));//authenticator
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));//for all roles(remove if using push&pop)
            Routing.RegisterRoute(nameof(ServicesPage), typeof(ServicesPage));//services page of customer's side
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));//forgotpassword page for all

            Routing.RegisterRoute(nameof(SuperAdminPage), typeof(SuperAdminPage));//for SuperAdminPage
            Routing.RegisterRoute(nameof(SuperAdminProfile), typeof(SuperAdminProfile));//for SuperAdminPage

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));//for adminpage
            Routing.RegisterRoute(nameof(AdminProfile), typeof(AdminProfile));//for adminProfile
            Routing.RegisterRoute(nameof(AdminChatPage), typeof(AdminChatPage));//for AdminChatPage
            //Routing.RegisterRoute(nameof(AdminSignupPage), typeof(AdminSignupPage));//for AdminSignupPage(remove if using push&pop)


            Routing.RegisterRoute(nameof(CustomerPage), typeof(CustomerPage));//for customer page
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));//customer profile
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));//for customer's ChatPage
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));//Default Signup Page for customer
            Routing.RegisterRoute(nameof(ContactInfoPage), typeof(ContactInfoPage));//contact infor of customer's side
            Routing.RegisterRoute(nameof(PaymentInfoPage), typeof(PaymentInfoPage));//payment info of customer's side
            Routing.RegisterRoute(nameof(AboutUsPage), typeof(AboutUsPage));//about of customer's page


        }
    }
}
