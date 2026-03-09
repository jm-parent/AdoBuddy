using AdoBuddy.Models;
using AdoBuddy.Services;
using AdoBuddy.ViewModels;

namespace AdoBuddy.Tests.ViewModels
{
    public class PipelinesViewModelTests
    {
        private static FakeAzureDevOpsService CreateService(List<PipelineRun>? runs = null) =>
            new() { PipelineRunsResult = runs ?? new List<PipelineRun>() };

        [Fact]
        public async Task LoadPipelines_PopulatesPipelineRuns()
        {
            var runs = new List<PipelineRun>
            {
                new() { Id = 1, PipelineName = "Build", Status = "completed", Result = "succeeded" },
                new() { Id = 2, PipelineName = "Deploy", Status = "inProgress", Result = null }
            };
            var vm = new PipelinesViewModel(CreateService(runs));

            await vm.LoadPipelinesCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.PipelineRuns.Count);
            Assert.Equal("Build", vm.PipelineRuns[0].PipelineName);
            Assert.Equal("Deploy", vm.PipelineRuns[1].PipelineName);
        }

        [Fact]
        public async Task LoadPipelines_ClearsPreviousResults()
        {
            var service = CreateService(new List<PipelineRun>
            {
                new() { Id = 1, PipelineName = "OldPipeline", Status = "completed" }
            });
            var vm = new PipelinesViewModel(service);
            await vm.LoadPipelinesCommand.ExecuteAsync(null);
            Assert.Single(vm.PipelineRuns);

            service.PipelineRunsResult = new List<PipelineRun>
            {
                new() { Id = 2, PipelineName = "NewPipeline", Status = "inProgress" },
                new() { Id = 3, PipelineName = "AnotherPipeline", Status = "completed" }
            };
            await vm.LoadPipelinesCommand.ExecuteAsync(null);

            Assert.Equal(2, vm.PipelineRuns.Count);
        }

        [Fact]
        public async Task LoadPipelines_IsBusyFalseAfterCompletion()
        {
            var vm = new PipelinesViewModel(CreateService());

            await vm.LoadPipelinesCommand.ExecuteAsync(null);

            Assert.False(vm.IsBusy);
        }

        [Fact]
        public async Task LoadPipelines_EmptyProject_ReturnsEmptyList()
        {
            var vm = new PipelinesViewModel(CreateService());
            vm.ProjectName = string.Empty;

            await vm.LoadPipelinesCommand.ExecuteAsync(null);

            Assert.Empty(vm.PipelineRuns);
        }

        [Fact]
        public void InitialState_IsBusyFalse_PipelineRunsEmpty()
        {
            var vm = new PipelinesViewModel(CreateService());

            Assert.False(vm.IsBusy);
            Assert.Empty(vm.PipelineRuns);
            Assert.Equal("Pipelines", vm.Title);
        }
    }
}
