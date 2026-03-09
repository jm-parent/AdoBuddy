using System.Collections.ObjectModel;
using AdoBuddy.Models;
using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
#if ANDROID || IOS || MACCATALYST || WINDOWS
    [Microsoft.Maui.Controls.QueryProperty(nameof(ProjectName), "projectName")]
    [Microsoft.Maui.Controls.QueryProperty(nameof(ProjectId), "projectId")]
#endif
    public partial class PullRequestsViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;

        public ObservableCollection<PullRequest> PullRequests { get; } = new();

        private string _projectName = string.Empty;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                Title = value;
                if (!string.IsNullOrEmpty(value))
                    LoadPullRequestsCommand.Execute(null);
            }
        }

        [ObservableProperty]
        public partial string ProjectId { get; set; }

        public PullRequestsViewModel(IAzureDevOpsService service)
        {
            _service = service;
            Title = "Pull Requests";
            ProjectId = string.Empty;
        }

        [RelayCommand]
        private async Task LoadPullRequestsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var prs = await _service.GetPullRequestsAsync(ProjectName);
                PullRequests.Clear();
                foreach (var pr in prs)
                    PullRequests.Add(pr);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

