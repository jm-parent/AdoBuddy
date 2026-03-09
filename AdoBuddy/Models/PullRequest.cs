namespace AdoBuddy.Models
{
    public class PullRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string TargetBranch { get; set; } = string.Empty;
        public string SourceBranch { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}
