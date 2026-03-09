using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;
        private readonly ICredentialStore _credentials;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        public partial string OrganizationUrl { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        public partial string Pat { get; set; }

        [ObservableProperty]
        public partial string ErrorMessage { get; set; }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));

        [ObservableProperty]
        public partial bool IsLoginSuccessful { get; set; }

        public LoginViewModel(IAzureDevOpsService service, ICredentialStore credentials)
        {
            _service = service;
            _credentials = credentials;
            OrganizationUrl = _credentials.GetOrgUrl() ?? string.Empty;
            Pat = string.Empty;
            ErrorMessage = string.Empty;
            Title = "Sign In";
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                var valid = await _service.ValidateConnectionAsync(OrganizationUrl, Pat);
                if (valid)
                {
                    _credentials.SaveOrgUrl(OrganizationUrl);
                    await _credentials.SavePatAsync(Pat);
                    IsLoginSuccessful = true;
                }
                else
                {
                    ErrorMessage = "Invalid organization URL or PAT. Please check your credentials.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Connection error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanLogin() =>
            !string.IsNullOrWhiteSpace(OrganizationUrl) && !string.IsNullOrWhiteSpace(Pat);
    }
}
