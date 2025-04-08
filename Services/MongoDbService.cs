using System.Diagnostics;
using Barangay_Office.Models;
using MongoDB.Bson;
using Microsoft.Maui.Storage;
using MongoDB.Driver;

namespace Barangay_Office.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CustomerServiceMessage> _CustomerServiceCollection;
        private IChangeStreamCursor<ChangeStreamDocument<CustomerServiceMessage>>? _changeStreamCursor;
        private readonly MongoClient _client;

        // Event triggered when a new message is received
        public event Action<CustomerServiceMessage> OnNewMessageReceived = delegate { };


        public MongoDbService()
        {
            try
            {
                // Retrieve connection string from SecureStorage
                var connectionString = "mongodb+srv://Taisho:AdminPassword1234568@barangayoffice.kkfy4.mongodb.net/?retryWrites=true&w=majority&appName=BarangayOffice";

                var settings = MongoClientSettings.FromConnectionString(connectionString);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);

                //create client and connect to servver
                _client = new MongoClient(settings);

                // Verify connection by listing databases
                var dbList = _client.ListDatabaseNames().ToList();
                Console.WriteLine("Databases: " + string.Join(", ", dbList));

                _database = _client.GetDatabase("ThisSQL");

                //check if the database is null
                if(_database == null)
                {
                    throw new Exception("MongoDB database instance is null. Please check your database name.");
                }

                _CustomerServiceCollection = _database.GetCollection<CustomerServiceMessage>("customer_service_database");
                //check if the collection is null
                if(_CustomerServiceCollection == null)
                {
                    throw new Exception("CustomerService collection is null. Please check your collection name.");
                }

                // Test connection
                var result = _database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. Successfully connected to MongoDB!");

                InitializeChangeStream().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _client = null!;
                _database = null!;
                _CustomerServiceCollection = null!;
            }
        }

        public IMongoCollection<CustomerServiceMessage> CustomerServiceMessages => _CustomerServiceCollection;

        public async Task CreateIndexes()
        {
            if(_CustomerServiceCollection == null)
            {
                throw new InvalidOperationException("MongoDB Customer Service Collection is not initialized. Check your connection settings.");
            }
            var indexKeysDefinition = Builders<CustomerServiceMessage>.IndexKeys.Ascending(x => x.Timestamp);
            await _CustomerServiceCollection.Indexes.CreateOneAsync(new CreateIndexModel<CustomerServiceMessage>(indexKeysDefinition));
        }


        public async Task SendMessageAsync(CustomerServiceMessage message)
        {
            if (_CustomerServiceCollection == null)
            {
                throw new InvalidOperationException("MongoDB Customer Service Collection is not initialized. Check your connection settings.");
            }
            await _CustomerServiceCollection.InsertOneAsync(message);
        }

        private async Task InitializeChangeStream()
        {
            try
            {
                var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<CustomerServiceMessage>>()
                    .Match(change => change.OperationType == ChangeStreamOperationType.Insert);

                var options = new ChangeStreamOptions { FullDocument = ChangeStreamFullDocumentOption.UpdateLookup };

                _changeStreamCursor = await _CustomerServiceCollection.WatchAsync(pipeline, options).ConfigureAwait(false);

                while (await _changeStreamCursor.MoveNextAsync().ConfigureAwait(false))
                {
                    foreach (var change in _changeStreamCursor.Current)
                    {
                        if (change.FullDocument != null)
                        {
                            Console.WriteLine($"New message detected: {change.FullDocument.Content}");
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                OnNewMessageReceived?.Invoke(change.FullDocument);
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Change Stream: {ex.Message}");
            }
        }

        public async Task<List<CustomerServiceMessage>> GetAllMessages()
        {

            if (_CustomerServiceCollection == null)
            {
                throw new InvalidOperationException("MongoDB Customer Service Collection is not initialized. Check your connection settings.");
            }

            try
            {
                return await _CustomerServiceCollection
                    .Find(_ => true)
                    .SortBy(m => m.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving messages: {ex.Message}");
                return new List<CustomerServiceMessage>();
            }
        }

        public void Dispose()
        {
            _changeStreamCursor?.Dispose();
        }

        // Method to store connection string securely (call this once when setting up)
        //public static async Task StoreConnectionString(string connectionString)
        //{
        //    await SecureStorage.SetAsync("mongo_connection_string", connectionString);
        //}
    }
}
