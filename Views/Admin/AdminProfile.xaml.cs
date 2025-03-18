using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using SQLite;

namespace Barangay_Office.Views;

public partial class AdminProfile : ContentPage
{
	private readonly AuthService _authService;
    public AdminProfile(AuthService authService)
	{
		InitializeComponent();
        _authService = authService;
        BindingContext = new AdminProfileViewModel();
    }
}