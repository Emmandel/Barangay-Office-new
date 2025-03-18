using Barangay_Office.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Barangay_Office.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<CustomerService> _CustomerServiceCollection;
        private IChangeStreamCursor<ChangeStreamDocument<CustomerService>> _changeStreamCursor;

        // Event triggered when a new message is received
        public event Action<CustomerService> OnNewMessageReceived;


        public MongoDbService()
        {
            try
            {
                const string connectionUrl = "mongodb+srv://Taisho:qrf1MinWjnwlGnIP@barangayoffice.kkfy4.mongodb.net/?retryWrites=true&w=majority&appName=BarangayOffice";

                var settings = MongoClientSettings.FromConnectionString(connectionUrl);

                settings.ServerApi = new ServerApi(ServerApiVersion.V1);

                // Create a new client and connect to the server
                var client = new MongoClient(settings);

                // Verify connection by listing databases
                var dbList = client.ListDatabaseNames().ToList();
                Console.WriteLine("Databases: " + string.Join(", ", dbList));

                _database = client.GetDatabase("ThisSQL");
                // Check if the database is null
                if (_database == null)
                {
                    throw new Exception("MongoDB database instance is null. Please check your database name.");
                }

                _CustomerServiceCollection = _database.GetCollection<CustomerService>("customer_service_database");
                // Check if the collection is null
                if (_CustomerServiceCollection == null)
                {
                    throw new Exception("CustomerService collection is null. Please check your collection name.");
                }


                // Test connection
                var result = _database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. Successfully connected to MongoDB!");

                InitializeChangeStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public IMongoCollection<CustomerService> CustomerMessages => _CustomerServiceCollection;

        public async Task CreateIndexes()
        {
            var indexKeysDefinition = Builders<CustomerService>.IndexKeys.Ascending(x => x.Timestamp);
            await _CustomerServiceCollection.Indexes.CreateOneAsync(new CreateIndexModel<CustomerService>(indexKeysDefinition));
        }

        public async Task SendMessageAsync(CustomerService message)
        {
            await _CustomerServiceCollection.InsertOneAsync(message);
        }

        private async void InitializeChangeStream()
        {
            try
            {
                var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<CustomerService>>().Match(change => change.OperationType == ChangeStreamOperationType.Insert);

                var options = new ChangeStreamOptions { FullDocument = ChangeStreamFullDocumentOption.UpdateLookup };

                using var cursorTask = _CustomerServiceCollection.WatchAsync(pipeline, options);
                using var cursor = await cursorTask;

                while (await cursor.MoveNextAsync())
                {
                    foreach (var change in cursor.Current)
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

        public async Task<List<CustomerService>> GetAllMessages()
        {
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
                return new List<CustomerService>();
            }
        }

        public void Dispose()
        {
            _changeStreamCursor?.Dispose();
        }
    }
}
