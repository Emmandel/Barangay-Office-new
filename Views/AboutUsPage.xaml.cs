using System.Threading.Tasks;

namespace Barangay_Office.Views;

public partial class AboutUsPage : ContentPage
{
	public AboutUsPage()
	{
		InitializeComponent();
	}

    private async void OnCustomerServiceClicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new ChatPage());
    }
}