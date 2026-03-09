using Microsoft.Maui.Storage;

namespace AdoBuddy.Services
{
    public class CredentialStore : ICredentialStore
    {
        private const string OrgUrlKey = "OrgUrl";
        private const string PatKey = "PAT";

        public void SaveOrgUrl(string url) =>
            Preferences.Default.Set(OrgUrlKey, url);

        public string? GetOrgUrl() =>
            Preferences.Default.ContainsKey(OrgUrlKey)
                ? Preferences.Default.Get(OrgUrlKey, string.Empty)
                : null;

        public async Task SavePatAsync(string pat) =>
            await SecureStorage.Default.SetAsync(PatKey, pat);

        public async Task<string?> GetPatAsync()
        {
            try { return await SecureStorage.Default.GetAsync(PatKey); }
            catch { return null; }
        }

        public bool HasCredentials() =>
            Preferences.Default.ContainsKey(OrgUrlKey);
    }
}
