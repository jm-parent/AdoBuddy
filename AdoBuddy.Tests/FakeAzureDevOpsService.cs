using AdoBuddy.Models;
using AdoBuddy.Services;

namespace AdoBuddy.Tests
{
    internal class FakeAzureDevOpsService : IAzureDevOpsService
    {
        public List<PipelineRun> PipelineRunsResult { get; set; } = new();
        public List<PullRequest> PullRequestsResult { get; set; } = new();
        public bool ValidateResult { get; set; } = true;

        public Task<bool> ValidateConnectionAsync(string orgUrl, string pat) =>
            Task.FromResult(ValidateResult);

        public Task<List<PipelineRun>> GetPipelineRunsAsync(string project) =>
            Task.FromResult(PipelineRunsResult);

        public Task<List<PullRequest>> GetPullRequestsAsync(string project) =>
            Task.FromResult(PullRequestsResult);
    }
}
