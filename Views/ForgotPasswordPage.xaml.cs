using Barangay_Office.Services;
using Barangay_Office.ViewModels;

namespace Barangay_Office.Views;

public partial class ForgotPasswordPage : ContentPage
{
	private readonly AuthService _authService;
	public ForgotPasswordPage(AuthService authService)
	{
		InitializeComponent();
		BindingContext = new ForgotPasswordViewModel(authService);
    }


    private void OntoggleFPasswordVisibility(object sender, EventArgs e)
    {
        NewPasswordEntry.IsPassword = !NewPasswordEntry.IsPassword;
        ((ImageButton)sender).Source = NewPasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }
}