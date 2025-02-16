using MongoDB.Bson;
using MongoDB.Driver;

namespace Barangay_Office
{
    public class MongoDBHelper
    {
        public MongoDBHelper()
        {
            const string connectionUrl = "mongodb+srv://Taisho:Emar@Home2@myweb.kkfy4.mongodb.net/?retryWrites=true&w=majority&appName=MyWeb";

            var settings = MongoClientSettings.FromConnectionString(connectionUrl);

            //set the server api field of the settings ofject
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            //connect to the server
            var client = new MongoClient(settings);

            //confirm a successful connection
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Connected Successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect" + e);
            }
        }
    }
}
