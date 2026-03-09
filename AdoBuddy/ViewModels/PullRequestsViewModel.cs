using System.Collections.ObjectModel;
using AdoBuddy.Models;
using AdoBuddy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AdoBuddy.ViewModels
{
    public partial class PullRequestsViewModel : BaseViewModel
    {
        private readonly IAzureDevOpsService _service;

        public ObservableCollection<PullRequest> PullRequests { get; } = new();

        [ObservableProperty]
        public partial string ProjectName { get; set; }

        [ObservableProperty]
        public partial string ProjectId { get; set; }

        public PullRequestsViewModel(IAzureDevOpsService service)
        {
            _service = service;
            Title = "Pull Requests";
            ProjectName = string.Empty;
            ProjectId = string.Empty;
        }

        partial void OnProjectNameChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                LoadPullRequestsCommand.Execute(null);
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
