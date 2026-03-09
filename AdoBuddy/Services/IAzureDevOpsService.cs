using AdoBuddy.Models;

namespace AdoBuddy.Services
{
    public interface IAzureDevOpsService
    {
        Task<bool> ValidateConnectionAsync(string orgUrl, string pat);
        Task<List<PipelineRun>> GetPipelineRunsAsync(string project);
        Task<List<PullRequest>> GetPullRequestsAsync(string project);
    }
}
