/* [grial-metadata] id: Grial#App.xaml version: 1.1.3 */
using UXDivers.Grial;
namespace Barangay_Office
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}