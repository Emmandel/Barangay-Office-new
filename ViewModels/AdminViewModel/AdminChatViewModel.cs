using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Barangay_Office.Models;
using Barangay_Office.Services;
using Barangay_Office.Views;
using Barangay_Office.Views.Admin;
using CommunityToolkit.Mvvm.Input;

namespace Barangay_Office.ViewModels
{
    public partial class AdminChatViewModel : BaseViewModel
    {
        private readonly ChatService _chatService;
        public ObservableCollection<CustomerServiceMessage> Messages { get; } = new ObservableCollection<CustomerServiceMessage>();

        private string _newMessage = string.Empty;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged();
            }
        }

        //commands
        public ICommand SendMessageCommand { get; }
        public ICommand BackButtonCommand { get; }

        public AdminChatViewModel()
        {
            _chatService = new ChatService(new MongoDbService());
            BackButtonCommand = new RelayCommand(async () =>
            {
                Console.WriteLine("Back Button works");
                await Shell.Current.GoToAsync($"//{nameof(AdminProfile)}");
            });

            SendMessageCommand = new AsyncRelayCommand(SendMessage);

            // Set admin as online when view appears
            _chatService.SetAdminStatus(true);

            //loading new messages and subscribe to real-time messages
            
            LoadMessages();
            SubscribeToMessages();
        }

        public void OnAppearing()
        {
            // Set admin as online when view appears
            _chatService.SetAdminStatus(true);
            LoadMessages();
            SubscribeToMessages();
        }

        public void OnDisappearing()
        {
            // Set admin as offline when view disappears
            _chatService.SetAdminStatus(false);
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

                var message = new CustomerServiceMessage
                {
                    Content = messageContent,
                    SenderID = "Admin",
                    SenderRole = "Admin",
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                // Add detailed logging before the call
                Debug.WriteLine($"About to send message: {message.Content}");
                Debug.WriteLine($"Message properties - SenderID: {message.SenderID}, SenderRole: {message.SenderRole}");

                // Try direct send first
                try
                {
                    await _chatService.SendMessageAsync(message);
                    Debug.WriteLine("Message sent successfully!");
                }
                catch (NullReferenceException)
                {
                    // If MongoDB service or collection is null, try alternative approach
                    Debug.WriteLine("Attempting to send message using alternative method...");
                    bool result = await _chatService.SendMessage(message.Content, "Admin");

                    if (result)
                    {
                        Debug.WriteLine("Message sent successfully using alternative method!");
                    }
                    else
                    {
                        throw new Exception("Failed to send message using alternative method");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log detailed error information
                Debug.WriteLine($"Exception type: {ex.GetType().Name}");
                Debug.WriteLine($"Error sending message: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // If there's an inner exception, log that too
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Debug.WriteLine($"Inner exception stack trace: {ex.InnerException.StackTrace}");
                }

                Console.WriteLine($"Ayaw mag send ng message: {ex.Message}");
            }
        }

        private void ScrollToBottom()
        {
            try
            {

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage is NavigationPage navPage)
                {
                    if (navPage.CurrentPage is AdminChatPage chatPage)
                    {
                        var messageCollection = chatPage.FindByName<CollectionView>("MessageCollection");
                        if (messageCollection != null)
                        {
                            var lastMessage = Messages.LastOrDefault();
                            if (lastMessage != null)
                            {
                                messageCollection.ScrollTo(lastMessage, position: ScrollToPosition.End, animate: true);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Error: CollectionView 'MessageCollection' not found in AdminChatPage.");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error: Current page is not AdminChatPage.");
                    }
                }
                else
                {
                    Debug.WriteLine("Error: MainPage is not a NavigationPage.");
                }
            });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error invoking MainThread in ScrollToBottom: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}