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

        protected override async void OnStart()
        {
            var DbService = Services.GetService<LocalDatabase>();
            await DbService.InitAsync();
        }
    }
}