using Barangay_Office.Models;
using MongoDB.Driver;

namespace Barangay_Office.Services
{
    public class ChatService
    {
        private readonly MongoDbService _mongoDbService;

        public event Action<CustomerService>OnNewMessageReceived;


        public ChatService(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            _mongoDbService.OnNewMessageReceived += (message) => OnNewMessageReceived?.Invoke(message);
        }

        // Get all messages sorted by timestamp
        public async Task<List<CustomerService>> GetCustomerServiceConversation()
        {
            try
            {
                var messages = await _mongoDbService.CustomerMessages
                    .Find(Builders<CustomerService>.Filter.Empty)
                    .SortBy(m => m.Timestamp)
                    .ToListAsync();

                Console.WriteLine($"Retrieved {messages.Count} messages");
                return messages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving messages: {ex.Message}");
                return new List<CustomerService>();
            }
        }

        public async Task<CustomerService> SendMessageToCustomerService(string messageContent)
        {
            try{
                string senderId = "Admin";
                string userEmail = Preferences.Get("UserEmail", string.Empty);

                if (!string.IsNullOrEmpty(userEmail) && userEmail != "Admin@gmail.com")
                {
                    senderId = userEmail;
                }
                Console.WriteLine($"Sending message as: {senderId}");

                var message = new CustomerService
                {
                    Content = messageContent,
                    SenderID = senderId,
                    SenderRole = senderId == "Admin" ? "Admin" : "Customer",
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _mongoDbService.SendMessageAsync(message);
                Console.WriteLine($"Message sent: {message.Content}");
                return message;
            }catch(Exception ex){
                Console.WriteLine($"Error sending message: {ex.Message}");
                return null;
            }
        }

        // subscribe to real-time updates
        public void SubscribeToCustomerServiceUpdates(Action<CustomerService> onNewMessage)
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

        public async Task SendMessageAsync(CustomerService message)
        {
            try{
                Console.WriteLine($"Sending message as {message.SenderID}: {message.Content}");
                await _mongoDbService.SendMessageAsync(message);
                Console.WriteLine("Message sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw;
            }
        }

        // Send a message base on role
        public async Task<bool> SendMessage(string content, string senderId)
        {
            try
            {
                Console.WriteLine($"Sending message as {senderId}: {content}");
                var message = new CustomerService
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


