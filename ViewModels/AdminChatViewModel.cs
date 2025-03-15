using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Barangay_Office.ViewModels
{
    public partial class AdminChatViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;

        public ObservableCollection<CustomerService> Messages { get; } = new();

        //commands
        public ICommand SendMessageCommand { get; }


        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set { _newMessage = value; }
        }

        public AdminChatViewModel(PostgreSqlService postgreSqlService)
        {
            _chatService = new ChatService(postgreSqlService);
            Messages = new ObservableCollection<CustomerService>();
            SendMessageCommand = new AsyncRelayCommand(SendMessage);
            LoadMessages();
        }

        private async void LoadMessages()
        {
            var messages = await _chatService.GetCustomerServiceConversation();

            foreach (var msg in messages)
            {
                Messages.Add(msg);
            }
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(NewMessage))
            {
                return;
            }
            var message = await _chatService.SendMessageToCustomerService(NewMessage);
            if (message != null)
            {
                Messages.Add(new CustomerService
                {
                    Content = NewMessage,
                    SenderID = Preferences.Get("UserEmail", string.Empty)
                });

                NewMessage = string.Empty;
                OnPropertyChanged(nameof(NewMessage));
            }
        }
    }
}