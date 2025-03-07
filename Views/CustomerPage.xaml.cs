namespace Barangay_Office.Views;

public partial class CustomerPage : ContentPage
{
	public CustomerPage()
	{
		InitializeComponent();
		BindingContext = new CustomerPageViewModel();
    }
}