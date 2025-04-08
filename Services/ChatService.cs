using Barangay_Office.Models;
using MongoDB.Driver;

namespace Barangay_Office.Services
{
    public class ChatService
    {
        private readonly MongoDbService _mongoDbService;

        public event Action<CustomerServiceMessage>? OnNewMessageReceived;


        public ChatService(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService ?? throw new ArgumentNullException(nameof(mongoDbService));
            _mongoDbService.OnNewMessageReceived += (message) => OnNewMessageReceived?.Invoke(message);
        }

        // Get all messages sorted by timestamp
        public async Task<List<CustomerServiceMessage>> GetCustomerServiceConversation()
        {
            try
            {
                var messages = await _mongoDbService.CustomerServiceMessages
                    .Find(Builders<CustomerServiceMessage>.Filter.Empty)
                    .SortBy(m => m.Timestamp)
                    .ToListAsync();

                Console.WriteLine($"Retrieved {messages.Count} messages");
                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving messages: {ex.Message}");
                return new List<CustomerServiceMessage>();
            }
        }

        private bool _isAdminOnline = false;
        private Timer? _botResponseTimer;

        public async Task<CustomerServiceMessage?> SendMessageToCustomerService(string messageContent)
        {
            try
            {
                string senderId = "Admin";
                string userEmail = Preferences.Get("UserEmail", string.Empty);

                if (!string.IsNullOrEmpty(userEmail) && userEmail != "Admin@gmail.com")
                {
                    senderId = userEmail;
                }

                var message = new CustomerServiceMessage
                {
                    Content = messageContent,
                    SenderID = senderId,
                    SenderRole = senderId == "Admin" ? "Admin" : "Customer",
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _mongoDbService.SendMessageAsync(message);

                // If customer sends message and admin is offline
                if (senderId != "Admin" && !_isAdminOnline)
                {
                    _botResponseTimer = new Timer(async _ => {
                        await SendBotResponse(message);
                    }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
                }

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                return null;
            }
        }

        private async Task SendBotResponse(CustomerServiceMessage originalMessage)
        {
            var botMessage = new CustomerServiceMessage
            {
                Content = "Thank you for your message. Our admin is currently unavailable. " +
                         "We will respond as soon as possible.",
                SenderID = "System",
                SenderRole = "System",
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            await _mongoDbService.SendMessageAsync(botMessage);
            OnNewMessageReceived?.Invoke(botMessage);
        }

        public void SetAdminStatus(bool isOnline)
        {
            _isAdminOnline = isOnline;
            if (_isAdminOnline && _botResponseTimer != null)
            {
                _botResponseTimer.Dispose();
            }
        }

        // subscribe to real-time updates
        public void SubscribeToCustomerServiceUpdates(Action<CustomerServiceMessage> onNewMessage)
        {
            try
            {
                Console.WriteLine("Setting up real-time subscription for customer service messages");
                OnNewMessageReceived += onNewMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up subscription: {ex.Message}");
            }
        }

        public async Task SendMessageAsync(CustomerServiceMessage message)
        {
            try
            {
                Console.WriteLine($"Sending message as {message.SenderID}: {message.Content}");
                await _mongoDbService.SendMessageAsync(message);
                Console.WriteLine("Message sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        // Send a message base on role
        public async Task<bool> SendMessage(string content, string senderId)
        {

            try
            {
                Console.WriteLine($"Sending message as {senderId}: {content}");
                var message = new CustomerServiceMessage
                {
                    Content = content,
                    Timestamp = DateTime.UtcNow,
                    SenderID = senderId,
                    SenderRole = senderId == "Admin" ? "Admin" : "Customer",
                    IsRead = false
                };

                await _mongoDbService.SendMessageAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                return false;
            }
        }
    }
}


