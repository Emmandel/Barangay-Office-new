using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.Views;

namespace Barangay_Office.ViewModels
{
    public class CustomerServiceViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;
        private const string ADMIN_EMAIL = "Admin@gmail.com";

        public ObservableCollection<CustomerService> Messages { get; set; }

        private string _messageEntry;
        private MongoDbService mongoDbService;

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

        //public CustomerServiceViewModel()
        //{
        //    Messages = new ObservableCollection<CustomerService>
        //    {
        //        new CustomerService { Content = MessageEntry, SenderID = "Admin", Timestamp = DateTime.UtcNow }
        //    };
        //}



        public ICommand SendCommand { get; }
        public ICommand BackButton { get; }
        public CustomerServiceViewModel() : this(new MongoDbService()) { }

        //public CustomerServiceViewModel() : this(new PostgreSqlService()) { }

        public CustomerServiceViewModel(ChatService chatService)
        {
            Messages = new ObservableCollection<CustomerService>();
            _chatService = chatService;

            SendCommand = new Command(async () => await SendMessageAsync());
            BackButton = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());

            //Load for existing message first
            LoadInitialMessages();
            //Listen for the new messages
            SubscribeToMessages();
        }

        public CustomerServiceViewModel(MongoDbService mongoDbService)
        {
            this.mongoDbService = mongoDbService;
        }

        private async void LoadInitialMessages()
        {
            try
            {
                var existingMessages = await _chatService.GetCustomerServiceConversation();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var message in existingMessages)
                    {
                        Messages.Add(message);
                    }

                    // Scroll to the bottom after loading messages
                    ScrollToBottom();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading messages: {ex.Message}");
            }
        }

        //display latest messages
        private void SubscribeToMessages()
        {
            _chatService.OnNewMessageReceived += (newMessage) =>
            {
                MainThread.BeginInvokeOnMainThread(() => {
                    if(!Messages.Any(m => m.Id == newMessage.Id)){
                        Messages.Add(newMessage);
                        ScrollToBottom();
                    }
                });
            };
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(MessageEntry))
                return;

            try
            {
                string messageContent = MessageEntry.Trim();
                MessageEntry = string.Empty;


                string userEmail = Preferences.Get("UserEmail", string.Empty);
                string senderId = !string.IsNullOrEmpty(userEmail) ? userEmail : "Customer";
                var message = new CustomerService{
                    Content = messageContent,
                    SenderID = senderId,
                    SenderRole = "Customer",
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _chatService.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                    Application.Current.MainPage.DisplayAlert("Error", $"Failed to send message: {ex.Message}", "OK"));
            }
        }

        private void ScrollToBottom()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current.MainPage is NavigationPage navPage &&
                    navPage.CurrentPage is ChatPage chatPage)
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
