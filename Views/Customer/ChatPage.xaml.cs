using Barangay_Office.Services;
using Barangay_Office.ViewModels;

namespace Barangay_Office.Views;

public partial class ChatPage : ContentPage
{
    private readonly ChatService _chatService;
    public ChatPage(ChatService chatService)
	{
		InitializeComponent();
        _chatService = chatService;
        BindingContext = new CustomerServiceViewModel(_chatService);
    }

}