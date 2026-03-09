using AdoBuddy.Models;
using AdoBuddy.Services;
using AdoBuddy.ViewModels;

namespace AdoBuddy.Tests.ViewModels
{
    public class PullRequestsViewModelTests
    {
        private static FakeAzureDevOpsService CreateService(List<PullRequest>? prs = null) =>
            new() { PullRequestsResult = prs ?? new List<PullRequest>() };

        [Fact]
        public void ProjectName_WhenSet_UpdatesTitle()
        {
            var vm = new PullRequestsViewModel(CreateService());

            vm.ProjectName = "MyProject";

            Assert.Equal("MyProject", vm.Title);
        }

        [Fact]
        public void ProjectName_WhenSetToNonEmpty_AutoLoadsPullRequests()
        {
            var prs = new List<PullRequest>
            {
                new() { Id = 1, Title = "Fix bug", CreatedBy = "Alice", TargetBranch = "refs/heads/main", SourceBranch = "refs/heads/fix/bug" },
                new() { Id = 2, Title = "Add feature", CreatedBy = "Bob", TargetBranch = "refs/heads/main", SourceBranch = "refs/heads/feature/x" }
            };
            var vm = new PullRequestsViewModel(CreateService(prs));

            vm.ProjectName = "MyProject";

            // FakeService is synchronous (Task.FromResult), so load completes before setter returns
            Assert.Equal(2, vm.PullRequests.Count);
            Assert.Equal("Fix bug", vm.PullRequests[0].Title);
            Assert.Equal("Bob", vm.PullRequests[1].CreatedBy);
        }

        [Fact]
        public void ProjectName_WhenSetToEmpty_DoesNotTriggerLoad()
        {
            var prs = new List<PullRequest>
            {
                new() { Id = 1, Title = "Some PR", CreatedBy = "Alice" }
            };
            var vm = new PullRequestsViewModel(CreateService(prs));

            vm.ProjectName = string.Empty;

            Assert.Empty(vm.PullRequests);
        }

        [Fact]
        public async Task LoadPullRequests_ClearsPreviousResults()
        {
            var service = CreateService(new List<PullRequest>
            {
                new() { Id = 1, Title = "Old PR", CreatedBy = "Alice" }
            });
            var vm = new PullRequestsViewModel(service);
            vm.ProjectName = "MyProject";
            Assert.Single(vm.PullRequests);

            service.PullRequestsResult = new List<PullRequest>
            {
                new() { Id = 2, Title = "New PR A", CreatedBy = "Bob" },
                new() { Id = 3, Title = "New PR B", CreatedBy = "Carol" }
            };
            await vm.LoadPullRequestsCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.PullRequests.Count);
        }

        [Fact]
        public async Task LoadPullRequests_IsBusyFalseAfterCompletion()
        {
            var vm = new PullRequestsViewModel(CreateService());

            await vm.LoadPullRequestsCommand.ExecuteAsync(null);

            Assert.False(vm.IsBusy);
        }

        [Fact]
        public void InitialState_IsBusyFalse_PullRequestsEmpty()
        {
            var vm = new PullRequestsViewModel(CreateService());

            Assert.False(vm.IsBusy);
            Assert.Empty(vm.PullRequests);
            Assert.Equal("Pull Requests", vm.Title);
        }
    }
}

