using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace Barangay_Office
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Add crash handler
            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                Console.WriteLine($"Native Crash: {args.Exception}");
                args.Handled = true;
            };
        }
    }
}
