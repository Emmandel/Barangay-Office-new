using Microsoft.Extensions.Configuration;

namespace Barangay_Office
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                var config = new ConfigurationBuilder()
                    .AddUserSecrets<App>()
                    .Build();

                await SecureStorage.SetAsync("mongo_connection",
                    config["DatabaseSettings:ConnectionString"]);
            });

            MainPage = new AppShell();
        }
    }
}