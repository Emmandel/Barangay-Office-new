using Barangay_Office.Services;
using Barangay_Office.ViewModels;
using Barangay_Office.Views;
using Microsoft.Extensions.Logging;
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

            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<SignupPage>();
            builder.Services.AddTransient<CustomerPage>();
            builder.Services.AddTransient<ForgotPasswordPage>();
            builder.Services.AddTransient<AdminProfile>();

            return builder.Build();
        }
    }
}
