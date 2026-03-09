using AdoBuddy.Services;

namespace AdoBuddy
{
    public partial class AppShell : Shell
    {
        private readonly ICredentialStore _credentialStore;

        public string CurrentProjectName { get; private set; } = string.Empty;

        public AppShell(ICredentialStore credentialStore)
        {
            _credentialStore = credentialStore;
            InitializeComponent();
        }

        protected override async void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            // Extract projectName from the current query string to update flyout title
            var uri = args.Current?.Location?.OriginalString ?? string.Empty;
            var queryStart = uri.IndexOf('?');
            if (queryStart >= 0)
            {
                var queryString = uri[(queryStart + 1)..];
                var pairs = queryString.Split('&');
                foreach (var pair in pairs)
                {
                    var kv = pair.Split('=');
                    if (kv.Length == 2 && kv[0] == "projectName")
                    {
                        CurrentProjectName = Uri.UnescapeDataString(kv[1]);
                        OnPropertyChanged(nameof(CurrentProjectName));
                        break;
                    }
                }
            }
        }

        public async Task NavigateToInitialPageAsync()
        {
            if (_credentialStore.HasCredentials())
                await GoToAsync("//ProjectsPage");
            else
                await GoToAsync("//LoginPage");
        }
    }
}
