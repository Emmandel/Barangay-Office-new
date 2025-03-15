
using System.Text.Json;
using Barangay_Office.Models;
using Supabase.Realtime;
using Supabase.Realtime.PostgresChanges;

namespace Barangay_Office.Services
{
    public class ChatService
    {
        private readonly Supabase.Client _supabaseClient;
        private RealtimeChannel? _channel;
        private const string TableName = "customer_service_messages";


        public ChatService(PostgreSqlService postgreSqlService)
        {
            _supabaseClient = postgreSqlService.GetClient();
        }


        // Get all messages sorted by timestamp
        public async Task<List<CustomerService>> GetCustomerServiceConversation()
        {
            var response = await _supabaseClient
                .From<CustomerService>()
                .Order("timestamp", Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();
            return response.Models;
        }

        // Send a message (Admin/Customer)
        public async Task<bool> SendMessage(string content, string senderId)
        {
            var message = new CustomerService
            {
                Content = content,
                Timestamp = DateTime.UtcNow,
                SenderID = senderId  // Change this based on actual user ID
            };

            var response = await _supabaseClient
                .From<CustomerService>()
                .Insert(message);

            return response.Models.Count > 0;
        }



        // Real-time subscription to new messages
        public void SubscribeToCustomerServiceUpdates(Action<CustomerService> onNewMessage)
        {
            // Ensure only one subscription is active
            if (_channel != null)
            {
                _supabaseClient.Realtime.Remove(_channel);
            }

            _channel = _supabaseClient.Realtime.Channel(TableName);
            void value(Supabase.Realtime.Interfaces.IRealtimeChannel sender, Supabase.Realtime.PostgresChanges.PostgresChangesResponse payload)
            {
                try
                {
                    Console.WriteLine($"New Message: {payload.ToString()}");

                    var message = payload.Model<CustomerService>();

                    if (message != null)
                    {
                        onNewMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Supabase Realtime] Error: {ex.Message}");
                }
            }
            _channel
                .AddPostgresChangeHandler(PostgresChangesOptions.ListenType.All, value);

            _channel.Subscribe();
        }

        internal async Task<CustomerService> SendMessageToCustomerService(string messageContent)
        {
            throw new NotImplementedException();
        }
    }
}


