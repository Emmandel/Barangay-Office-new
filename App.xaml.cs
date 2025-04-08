using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;
namespace Barangay_Office
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            return window;
        }
    }
}