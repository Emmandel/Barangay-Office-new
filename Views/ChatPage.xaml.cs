using Barangay_Office.ViewModels;

namespace Barangay_Office.Views;

public partial class ChatPage : ContentPage
{
    
    public ChatPage()
	{
		InitializeComponent();
        BindingContext = new CustomerServiceViewModel();
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Scroll to the bottom when the page appears
    //    if (BindingContext is CustomerServiceViewModel viewModel)
    //    {
    //        MainThread.BeginInvokeOnMainThread(() =>
    //        {
    //            var lastMessage = viewModel.Messages.LastOrDefault();
    //            if (lastMessage != null)
    //            {
    //                MessageCollection?.ScrollTo(lastMessage, position: ScrollToPosition.End, animate: false);
    //            }
    //        });
    //    }
    //}
}