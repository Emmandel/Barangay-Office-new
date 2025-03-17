using System.Windows.Input;
using Barangay_Office.Services;
using Barangay_Office.Views;
using CommunityToolkit.Mvvm.Input;

namespace Barangay_Office.ViewModels
{
    public class AdminProfileViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        public ICommand ToAdminSignup { get; }
        public ICommand ToCustomerService { get; }
        public ICommand ToLogout { get; }

        public AdminProfileViewModel() : this(new AuthService()) { }

        public AdminProfileViewModel(AuthService authService)
        {
            _authService = authService;
            //go to admin signup
            ToAdminSignup = new Command(async () =>
            {
                _authService.LogOut();
                await Application.Current.MainPage.Navigation.PushAsync(new AdminSignupPage(_authService));
            });

            //go to customer service
            ToCustomerService = new RelayCommand(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AdminChatPage());
            });

            //go to logout
            ToLogout = new Command(() =>
            {
                _authService.LogOut();
                Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            });
        }
    }
}
