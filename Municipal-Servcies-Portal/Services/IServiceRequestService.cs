using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Services
{
    public interface IServiceRequestService
    {
        /// <summary>
        /// Initializes data structures from the database.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Gets all service requests for display.
        /// </summary>
        Task<List<ServiceRequestListViewModel>> GetAllRequestsAsync();

        /// <summary>
        /// Gets a single request using BST for O(log n) retrieval.
        /// Includes dependencies using Graph traversal.
        /// </summary>
        Task<ServiceRequestDetailViewModel?> GetRequestDetailsAsync(int id);

        /// <summary>
        /// Gets requests sorted by priority using MinHeap.
        /// </summary>
        Task<List<ServiceRequestListViewModel>> GetRequestsByPriorityAsync();

        /// <summary>
        /// Gets all dependencies for a request using Graph BFS.
        /// </summary>
        Task<List<Issue>> GetRequestDependenciesAsync(int id);
        
        /// <summary>
        /// Searches requests by ID (using BST), text, or category filter.
        /// </summary>
        Task<ServiceRequestSearchViewModel> SearchRequestsAsync(string? searchTerm, string? category);
        
        /// <summary>
        /// Gets list of all unique categories for filter dropdown.
        /// </summary>
        Task<List<string>> GetAllCategoriesAsync();
    }
}