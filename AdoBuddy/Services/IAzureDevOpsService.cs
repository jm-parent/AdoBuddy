using AdoBuddy.Models;

namespace AdoBuddy.Services
{
    public interface IAzureDevOpsService
    {
        Task<bool> ValidateConnectionAsync(string orgUrl, string pat);
        Task<List<AzureDevOpsProject>> GetProjectsAsync();
        Task<List<WorkItem>> GetWorkItemsAsync(string project);
        Task<List<PipelineRun>> GetPipelineRunsAsync(string project);
        Task<List<PullRequest>> GetPullRequestsAsync(string project);
    }
}
