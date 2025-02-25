using Barangay_Office.Services;

namespace Barangay_Office.Views;

public partial class ForgotPasswordPage : ContentPage
{
	private readonly AuthService _authService;
	public ForgotPasswordPage()
	{
		InitializeComponent();
		_authService = new AuthService();
	}

    private async void OnResetPassword(object sender, EventArgs e)
    {
		string email = EmailEntry.Text;
		string newPassword = NewPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
        {
            await DisplayAlert("Error", "All fields are required!", "OK");
            return;
        }

        bool isEmailRegistered = await _authService.IsEmailRegisteredAsync(email);

        if (!isEmailRegistered)
        {
            await DisplayAlert("Error", "Email is not registered!", "OK");
            return;
        }

        bool isReset = await _authService.ResetPasswordAsync(email, newPassword);

        if (isReset)
        {
            await DisplayAlert("Success", "Password has been reset successfully!", "OK");
            await Navigation.PopAsync(); // Navigate back to login
        }
        else
        {
            await DisplayAlert("Error", "Something went wrong!", "OK");
        }
    }

    private void OntoggleFPasswordVisibility(object sender, EventArgs e)
    {
        NewPasswordEntry.IsPassword = !NewPasswordEntry.IsPassword;
        ((ImageButton)sender).Source = NewPasswordEntry.IsPassword ? "close_eye.png" : "open_eye.png";
    }
}