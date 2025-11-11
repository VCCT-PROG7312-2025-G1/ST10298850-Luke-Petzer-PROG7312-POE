namespace Municipal_Servcies_Portal.ViewModels
{
    /// <summary>
    /// View model for search and filter results
    /// Wraps the search results with filter parameters
    /// </summary>
    public class ServiceRequestSearchViewModel
    {
        public List<ServiceRequestListViewModel> Results { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? CategoryFilter { get; set; }
        public List<string> AvailableCategories { get; set; } = new();
        public int TotalResults { get; set; }
        
        // Helper to check if any filters are active
        public bool IsFiltered => !string.IsNullOrEmpty(SearchTerm) || !string.IsNullOrEmpty(CategoryFilter);
    }
}

