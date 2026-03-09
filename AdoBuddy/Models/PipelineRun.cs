namespace AdoBuddy.Models
{
    public class PipelineRun
    {
        public int Id { get; set; }
        public string PipelineName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Result { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
