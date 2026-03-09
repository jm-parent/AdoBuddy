namespace AdoBuddy
{
    public partial class App : Application
    {
        private readonly AppShell _shell;

        public App(AppShell shell)
        {
            InitializeComponent();
            _shell = shell;
        }

        protected override Window CreateWindow(IActivationState? activationState) =>
            new Window(_shell);

        protected override async void OnStart()
        {
            base.OnStart();
            await _shell.NavigateToInitialPageAsync();
        }
    }
}