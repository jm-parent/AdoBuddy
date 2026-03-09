using AdoBuddy.Models;
using AdoBuddy.ViewModels;

namespace AdoBuddy.Tests.ViewModels
{
    public class ProjectsViewModelTests
    {
        private static FakeAzureDevOpsService CreateService(List<AzureDevOpsProject>? projects = null) =>
            new() { ProjectsResult = projects ?? [] };

        [Fact]
        public void InitialState_ProjectsEmpty_AndNotBusy()
        {
            var vm = new ProjectsViewModel(CreateService());
            Assert.Empty(vm.Projects);
            Assert.False(vm.IsBusy);
            Assert.Equal("Select a Project", vm.Title);
        }

        [Fact]
        public async Task LoadProjects_PopulatesProjects()
        {
            var projects = new List<AzureDevOpsProject>
            {
                new() { Id = "id1", Name = "Alpha", Description = "First project" },
                new() { Id = "id2", Name = "Beta",  Description = "Second project" }
            };
            var vm = new ProjectsViewModel(CreateService(projects));

            await vm.LoadProjectsCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.Projects.Count);
            Assert.Equal("Alpha", vm.Projects[0].Name);
            Assert.Equal("Beta", vm.Projects[1].Name);
        }

        [Fact]
        public async Task LoadProjects_ClearsPreviousResults()
        {
            var svc = CreateService([new() { Id = "id1", Name = "Old" }]);
            var vm = new ProjectsViewModel(svc);
            await vm.LoadProjectsCommand.ExecuteAsync(null);
            Assert.Single(vm.Projects);

            svc.ProjectsResult = [new() { Id = "id2", Name = "New1" }, new() { Id = "id3", Name = "New2" }];
            await vm.LoadProjectsCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.Projects.Count);
        }

        [Fact]
        public async Task LoadProjects_OnException_SetsErrorMessage()
        {
            var svc = new FakeAzureDevOpsService { ShouldThrow = true };
            var vm = new ProjectsViewModel(svc);

            await vm.LoadProjectsCommand.ExecuteAsync(null);

            Assert.NotEmpty(vm.ErrorMessage);
            Assert.Empty(vm.Projects);
        }

        [Fact]
        public async Task LoadProjects_ResetsIsBusy_AfterCompletion()
        {
            var vm = new ProjectsViewModel(CreateService());

            await vm.LoadProjectsCommand.ExecuteAsync(null);

            Assert.False(vm.IsBusy);
        }

        [Fact]
        public async Task LoadProjects_ClearsErrorMessage_OnRetry()
        {
            var svc = new FakeAzureDevOpsService { ShouldThrow = true };
            var vm = new ProjectsViewModel(svc);
            await vm.LoadProjectsCommand.ExecuteAsync(null);
            Assert.NotEmpty(vm.ErrorMessage);

            svc.ShouldThrow = false;
            svc.ProjectsResult = [new() { Id = "id1", Name = "Alpha" }];
            await vm.LoadProjectsCommand.ExecuteAsync(null);

            Assert.Empty(vm.ErrorMessage);
            Assert.Single(vm.Projects);
        }
    }
}
