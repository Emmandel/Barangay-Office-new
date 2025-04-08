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

        private string _messageEntry;

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

        private void SendMessage(object parameter)
        {
            //implement the send message
        }
    }
}
