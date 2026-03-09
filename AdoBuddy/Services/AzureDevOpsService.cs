using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AdoBuddy.Models;

namespace AdoBuddy.Services
{
    public class AzureDevOpsService : IAzureDevOpsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public AzureDevOpsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ValidateConnectionAsync(string orgUrl, string pat)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                SetBasicAuth(client, pat);
                var response = await client.GetAsync($"{orgUrl.TrimEnd('/')}/_apis/projects?api-version=7.1");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<PipelineRun>> GetPipelineRunsAsync(string project)
        {
            var client = await CreateAuthenticatedClientAsync();
            var runs = new List<PipelineRun>();

            var pipelinesResponse = await client.GetAsync($"{project}/_apis/pipelines?api-version=7.1");
            pipelinesResponse.EnsureSuccessStatusCode();

            var pipelinesJson = await pipelinesResponse.Content.ReadAsStringAsync();
            var pipelines = JsonSerializer.Deserialize<ListResponse<PipelineDto>>(pipelinesJson, JsonOptions);

            if (pipelines?.Value is null)
                return runs;

            foreach (var pipeline in pipelines.Value)
            {
                var runsResponse = await client.GetAsync($"{project}/_apis/pipelines/{pipeline.Id}/runs?api-version=7.1");
                if (!runsResponse.IsSuccessStatusCode)
                    continue;

                var runsJson = await runsResponse.Content.ReadAsStringAsync();
                var pipelineRuns = JsonSerializer.Deserialize<ListResponse<PipelineRunDto>>(runsJson, JsonOptions);

                if (pipelineRuns?.Value is null)
                    continue;

                foreach (var run in pipelineRuns.Value)
                {
                    runs.Add(new PipelineRun
                    {
                        Id = run.Id,
                        PipelineName = pipeline.Name,
                        Status = run.State ?? string.Empty,
                        Result = run.Result,
                        StartTime = run.CreatedDate,
                        FinishTime = run.FinishedDate
                    });
                }
            }

            return runs;
        }

        public async Task<List<PullRequest>> GetPullRequestsAsync(string project)
        {
            var client = await CreateAuthenticatedClientAsync();

            var response = await client.GetAsync(
                $"{project}/_apis/git/pullrequests?searchCriteria.status=active&api-version=7.1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ListResponse<PullRequestDto>>(json, JsonOptions);

            if (result?.Value is null)
                return new List<PullRequest>();

            return result.Value.Select(dto => new PullRequest
            {
                Id = dto.PullRequestId,
                Title = dto.Title,
                Status = dto.Status,
                CreatedBy = dto.CreatedBy?.DisplayName ?? string.Empty,
                TargetBranch = dto.TargetRefName,
                SourceBranch = dto.SourceRefName,
                CreationDate = dto.CreationDate
            }).ToList();
        }

        private async Task<HttpClient> CreateAuthenticatedClientAsync()
        {
            var orgUrl = Preferences.Get("OrgUrl", string.Empty);
            var pat = await SecureStorage.GetAsync("PAT") ?? string.Empty;

            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(pat))
                SetBasicAuth(client, pat);

            if (!string.IsNullOrEmpty(orgUrl))
                client.BaseAddress = new Uri(orgUrl.TrimEnd('/') + "/");

            return client;
        }

        private static void SetBasicAuth(HttpClient client, string pat)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}"));
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);
        }

        // Private DTOs used only for JSON deserialization

        private class ListResponse<T>
        {
            [JsonPropertyName("value")]
            public List<T> Value { get; set; } = new();
        }

        private class PipelineDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }

        private class PipelineRunDto
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("state")]
            public string? State { get; set; }

            [JsonPropertyName("result")]
            public string? Result { get; set; }

            [JsonPropertyName("createdDate")]
            public DateTime? CreatedDate { get; set; }

            [JsonPropertyName("finishedDate")]
            public DateTime? FinishedDate { get; set; }
        }

        private class PullRequestDto
        {
            [JsonPropertyName("pullRequestId")]
            public int PullRequestId { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;

            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;

            [JsonPropertyName("createdBy")]
            public IdentityRefDto? CreatedBy { get; set; }

            [JsonPropertyName("targetRefName")]
            public string TargetRefName { get; set; } = string.Empty;

            [JsonPropertyName("sourceRefName")]
            public string SourceRefName { get; set; } = string.Empty;

            [JsonPropertyName("creationDate")]
            public DateTime CreationDate { get; set; }
        }

        private class IdentityRefDto
        {
            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; } = string.Empty;
        }
    }
}
