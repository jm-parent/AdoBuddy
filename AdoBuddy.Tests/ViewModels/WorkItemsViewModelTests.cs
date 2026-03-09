using AdoBuddy.Models;
using AdoBuddy.ViewModels;

namespace AdoBuddy.Tests.ViewModels
{
    public class WorkItemsViewModelTests
    {
        private static FakeAzureDevOpsService CreateService(List<WorkItem>? items = null) =>
            new() { WorkItemsResult = items ?? [] };

        [Fact]
        public void InitialState_WorkItemsEmpty_NotBusy()
        {
            var vm = new WorkItemsViewModel(CreateService());
            Assert.Empty(vm.WorkItems);
            Assert.False(vm.IsBusy);
            Assert.Equal("Backlog", vm.Title);
        }

        [Fact]
        public async Task LoadWorkItems_PopulatesCollection()
        {
            var items = new List<WorkItem>
            {
                new() { Id = 1, Title = "Bug #1", State = "Active", WorkItemType = "Bug" },
                new() { Id = 2, Title = "Task #1", State = "Closed", WorkItemType = "Task" }
            };
            var vm = new WorkItemsViewModel(CreateService(items));
            vm.ProjectName = "MyProject";

            await vm.LoadWorkItemsCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.WorkItems.Count);
            Assert.Equal("Bug #1", vm.WorkItems[0].Title);
        }

        [Fact]
        public void SetProjectName_NonEmpty_TriggersLoad()
        {
            var svc = CreateService([new() { Id = 1, Title = "Auto-loaded" }]);
            var vm = new WorkItemsViewModel(svc);

            vm.ProjectName = "TriggerProject";

            // Give time for the synchronous command execution to complete
            Assert.Single(vm.WorkItems);
        }

        [Fact]
        public async Task LoadWorkItems_ResetsIsBusy()
        {
            var vm = new WorkItemsViewModel(CreateService());
            vm.ProjectName = "P";

            await vm.LoadWorkItemsCommand.ExecuteAsync(null);

            Assert.False(vm.IsBusy);
        }

        [Fact]
        public async Task LoadWorkItems_OnException_SetsErrorMessage()
        {
            var svc = new FakeAzureDevOpsService { ShouldThrow = true };
            var vm = new WorkItemsViewModel(svc);
            vm.ProjectName = "P"; // triggers load, but svc.GetWorkItems throws
            // wait for the load triggered by ProjectName setter
            await Task.Delay(50);

            Assert.NotEmpty(vm.ErrorMessage);
        }
    }
}
