namespace Municipal_Servcies_Portal.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DateReported { get; set; }
        public List<string> AttachmentPaths { get; set; } = new();
        public string? NotificationEmail { get; set; }
        public string? NotificationPhone { get; set; }
    }
}

