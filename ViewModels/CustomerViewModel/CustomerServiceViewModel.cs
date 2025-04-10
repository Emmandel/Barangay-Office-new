using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.Utilities;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class CustomerServiceViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;

        public ObservableCollection<CustomerServiceMessage> Messages { get; set; }

        private string _messageEntry = string.Empty;

        //accessors
        public string MessageEntry
        {
            get => _messageEntry;
            set
            {
                _messageEntry = value;
                OnPropertyChanged();
            }
        }



        public ICommand SendCommand { get; }
        public ICommand BackButton { get; }

        public CustomerServiceViewModel(ChatService chatService)
        {
            Messages = new ObservableCollection<CustomerServiceMessage>();
            _chatService = chatService;

            SendCommand = new RelayCommand(SendMessage);

            BackButton = new RelayCommand(async Bb =>
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutUsPage)}");
            });
        }

        private async Task SendMessage(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(MessageEntry))
            {
                // Add the message to the local collection
                var newMessage = new CustomerServiceMessage
                {
                    Content = MessageEntry,
                    IsUserMessage = true
                };
                Messages.Add(newMessage);

                // Clear the input field
                MessageEntry = string.Empty;

                // Simulate sending the message via the ChatService
                await _chatService.SendMessageAsync(newMessage);

                // Optionally, simulate receiving a response
                _chatService.SubscribeToCustomerServiceUpdates(response =>
                {
                    if(response != null)
                    {

                        Messages.Add(new CustomerServiceMessage
                        {
                            Content = response.Content,
                            IsUserMessage = false
                        });
                    }
                });
            }
        }
    }
}
