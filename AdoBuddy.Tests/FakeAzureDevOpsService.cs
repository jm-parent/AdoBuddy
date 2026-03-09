using AdoBuddy.Models;
using AdoBuddy.Services;

namespace AdoBuddy.Tests
{
    internal class FakeAzureDevOpsService : IAzureDevOpsService
    {
        public List<PipelineRun> PipelineRunsResult { get; set; } = new();
        public List<PullRequest> PullRequestsResult { get; set; } = new();
        public List<AzureDevOpsProject> ProjectsResult { get; set; } = new();
        public List<WorkItem> WorkItemsResult { get; set; } = new();
        public bool ValidateResult { get; set; } = true;
        public bool ShouldThrow { get; set; } = false;

        public Task<bool> ValidateConnectionAsync(string orgUrl, string pat)
        {
            if (ShouldThrow) throw new HttpRequestException("Network error");
            return Task.FromResult(ValidateResult);
        }

        public Task<List<AzureDevOpsProject>> GetProjectsAsync()
        {
            if (ShouldThrow) throw new HttpRequestException("Network error");
            return Task.FromResult(ProjectsResult);
        }

        public Task<List<WorkItem>> GetWorkItemsAsync(string project)
        {
            if (ShouldThrow) throw new HttpRequestException("Network error");
            return Task.FromResult(WorkItemsResult);
        }

        public Task<List<PipelineRun>> GetPipelineRunsAsync(string project) =>
            Task.FromResult(PipelineRunsResult);

        public Task<List<PullRequest>> GetPullRequestsAsync(string project) =>
            Task.FromResult(PullRequestsResult);
    }
}
