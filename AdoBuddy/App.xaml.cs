namespace AdoBuddy
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = IPlatformApplication.Current!.Services.GetRequiredService<AppShell>();
            return new Window(shell);
        }

        protected override async void OnStart()
        {
            base.OnStart();
            var shell = IPlatformApplication.Current!.Services.GetRequiredService<AppShell>();
            await shell.NavigateToInitialPageAsync();
        }
    }
}