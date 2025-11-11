namespace Municipal_Servcies_Portal.ViewModels
{
    public class ServiceRequestListViewModel
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime DateReported { get; set; }
        public string? Status { get; set; }
        public int Priority { get; set; }
        
        // Reference number for display
        public string ReferenceNumber => $"#MSP-2025-{Id:D6}";
        
        // Badge classes for status display
        public string StatusBadgeClass => Status switch
        {
            "Pending" => "bg-warning",
            "InProgress" => "bg-info",
            "Resolved" => "bg-success",
            "Closed" => "bg-secondary",
            _ => "bg-light"
        };
        
        // Badge classes for priority display
        public string PriorityBadgeClass => Priority switch
        {
            1 => "bg-danger",
            2 => "bg-warning",
            3 => "bg-info",
            4 => "bg-secondary",
            _ => "bg-light"
        };
    }
}