using AdoBuddy.Services;

namespace AdoBuddy.Tests
{
    internal class FakeCredentialStore : ICredentialStore
    {
        public string? StoredOrgUrl { get; private set; }
        public string? StoredPat { get; private set; }

        public void SaveOrgUrl(string url) => StoredOrgUrl = url;
        public string? GetOrgUrl() => StoredOrgUrl;
        public Task SavePatAsync(string pat) { StoredPat = pat; return Task.CompletedTask; }
        public Task<string?> GetPatAsync() => Task.FromResult(StoredPat);
        public bool HasCredentials() => StoredOrgUrl != null;
    }
}
