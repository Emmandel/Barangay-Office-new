using Barangay_Office.ViewModels;

namespace Barangay_Office.Views;

public partial class AdminChatPage : ContentPage
{
	public AdminChatPage(AdminChatViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Scroll to the bottom when the page appears
        if (BindingContext is AdminChatViewModel viewModel)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var lastMessage = viewModel.Messages.LastOrDefault();
                if (lastMessage != null)
                {
                    MessageCollection?.ScrollTo(lastMessage, position: ScrollToPosition.End, animate: false);
                }
            });
        }
    }
}