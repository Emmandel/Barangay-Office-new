using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Services;

namespace Barangay_Office.ViewModels
{
    public class CustomerServiceViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;
        private const string ADMIN_EMAIL = "Admin@gmail.com";

        public ObservableCollection<CustomerService> Messages { get; set; }

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

        public CustomerServiceViewModel(PostgreSqlService postgreSqlService)
        {
            Messages = new ObservableCollection<CustomerService>();
            _chatService = new ChatService(postgreSqlService);

            SendCommand = new Command(async () => await SendMessageAsync());
            BackButton = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

            //properly await initialization
            MainThread.BeginInvokeOnMainThread(() => ListenForNewMessages());
        }
        private void ListenForNewMessages()
        {
            _chatService.SubscribeToCustomerServiceUpdates((newMessage) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(newMessage);
                    ScrollToBottom();
                });
            });
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageEntry))
                return;

            try
            {
                string messageContent = MessageEntry;
                MessageEntry = string.Empty; // Clear input immediately

                var newMessage = await _chatService.SendMessageToCustomerService(messageContent);
                if (newMessage != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Messages.Add(newMessage);
                        ScrollToBottom();
                    });
                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current.MainPage.DisplayAlert("Error", $"Failed to send message: {ex.Message}", "OK"));
            }
        }

        private void ScrollToBottom()
        {
            var lastMessage = Messages.LastOrDefault();
            if (lastMessage != null)
            {
                var collection = Application.Current.MainPage?.FindByName<CollectionView>("MessageCollection");
                collection?.ScrollTo(lastMessage, position: ScrollToPosition.End, animate: true);
            }
        }
    }
}
