using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using Barangay_Office.Views;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;
using SQLite;

namespace Barangay_Office
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //use with dependency injection
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);

#if DEBUG
            builder.Logging.AddDebug();
#endif
            //Register SQLite as singleton
            builder.Services.AddSingleton<SQLiteAsyncConnection>(_ =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "barangay_office.db");
                return new SQLiteAsyncConnection(dbPath);
            });

            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<ChatService>();
            builder.Services.AddSingleton<MongoDbService>();

            builder.Services.AddTransient<ForgotPasswordPage>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddTransient<AboutUsPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<ContactInfoPage>();
            builder.Services.AddTransient<CustomerPage>();
            builder.Services.AddTransient<CustomerServiceViewModel>();
            builder.Services.AddTransient<PaymentInfoPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<SignupPage>();

            builder.Services.AddTransient<AdminChatPage>();
            builder.Services.AddTransient<AdminChatViewModel>();
            builder.Services.AddTransient<AdminProfile>();
            

            builder.Services.AddTransient<SuperAdminProfile>();

            return builder.Build();
        }
    }
}
