using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.ViewModels
{
    public class ServiceRequestDetailViewModel
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime DateReported { get; set; }
        public string? Status { get; set; }
        public int Priority { get; set; }
        public List<string> AttachmentPaths { get; set; } = new(); // ✅ Changed to list to match Issue model
        
        // Graph data - dependencies
        public List<Issue> DependencyIssues { get; set; } = new();
    }
}