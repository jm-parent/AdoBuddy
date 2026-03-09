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
        public partial string SelectedProject { get; set; }

        public PullRequestsViewModel(IAzureDevOpsService service)
        {
            _service = service;
            Title = "Pull Requests";
            SelectedProject = string.Empty;
        }

        [RelayCommand]
        private async Task LoadPullRequestsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var prs = await _service.GetPullRequestsAsync(SelectedProject);
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
