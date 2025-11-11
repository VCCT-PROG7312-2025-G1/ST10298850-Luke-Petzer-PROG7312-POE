namespace Municipal_Servcies_Portal.ViewModels
{
    public class DashboardStatsViewModel
    {
        public int IssuesResolved { get; set; }
        public int ActiveRequests { get; set; }
        public string AverageResponseTime { get; set; } = "N/A";
    }
}

