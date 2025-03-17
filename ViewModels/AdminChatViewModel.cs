using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.Views;
using CommunityToolkit.Mvvm.Input;

namespace Barangay_Office.ViewModels
{
    public partial class AdminChatViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;
        public ObservableCollection<CustomerService> Messages { get; } = [];

        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set { 
                _newMessage = value; 
                OnPropertyChanged();
            }
        }
        
        //commands
        public ICommand SendMessageCommand { get; }

        public AdminChatViewModel(ChatService chatService)
        {
            _chatService = chatService;
            Messages = new ObservableCollection<CustomerService>();
            SendMessageCommand = new AsyncRelayCommand(SendMessage);

            //loading new messages and subscribe to real-time messages
            LoadMessages();
            SubscribeToMessages();
        }

        private async void LoadMessages()
        {
            try
            {
                var messages = await _chatService.GetCustomerServiceConversation();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Messages.Clear();
                    foreach (var msg in messages)
                    {
                        Messages.Add(msg);
                    }
                    ScrollToBottom();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading messages: {ex.Message}");
            }
        }

        private void SubscribeToMessages()
        {
            _chatService.OnNewMessageReceived += (newMessage) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Only add the message if it's not already in the collection
                    if (!Messages.Any(m => m.Id == newMessage.Id))
                    {
                        Messages.Add(newMessage);
                        ScrollToBottom();
                    }
                });
            };
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
            {
                return;
            }

            try
            {
                string messageContent = NewMessage.Trim();
                NewMessage = string.Empty; // Clear input immediately

                var message = new CustomerService
                {
                    Content = messageContent,
                    SenderID = "Admin",
                    SenderRole = "Admin",
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _chatService.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to send message: {ex.Message}", "OK");
            }
        }

        private void ScrollToBottom()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current.MainPage is NavigationPage navPage &&
                    navPage.CurrentPage is AdminChatPage chatPage)
                {
                    var lastMessage = Messages.LastOrDefault();
                    if (lastMessage != null)
                    {
                        chatPage.FindByName<CollectionView>("MessageCollection")
                            ?.ScrollTo(lastMessage, position: ScrollToPosition.End, animate: true);
                    }
                }
            });
        }
    }
}