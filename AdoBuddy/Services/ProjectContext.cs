namespace AdoBuddy.Services
{
    /// <summary>Singleton shared state for the currently selected ADO project.</summary>
    public class ProjectContext
    {
        public string ProjectId { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
    }
}
