namespace Barangay_Office.Views;

public partial class PaymentInfoPage : ContentPage
{
	public PaymentInfoPage()
	{
		InitializeComponent();
	}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Navigate back to the previous page
    }
}