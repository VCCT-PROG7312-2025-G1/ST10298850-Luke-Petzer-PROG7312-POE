namespace Municipal_Servcies_Portal.ViewModels
{
    public class ServiceRequestListViewModel
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public DateTime DateReported { get; set; }
        public string? Status { get; set; }
        public int Priority { get; set; }
    }
}