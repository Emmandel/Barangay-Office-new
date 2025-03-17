using Barangay_Office.ViewModels;

namespace Barangay_Office.Views;

public partial class AdminChatPage : ContentPage
{
	public AdminChatPage()
	{
		InitializeComponent();
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