namespace AdoBuddy.Services
{
    public interface ICredentialStore
    {
        void SaveOrgUrl(string url);
        string? GetOrgUrl();
        Task SavePatAsync(string pat);
        Task<string?> GetPatAsync();
        bool HasCredentials();
    }
}
