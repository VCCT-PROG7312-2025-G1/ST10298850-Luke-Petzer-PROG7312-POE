using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Services;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestService _serviceRequestService;
        private readonly ILogger<ServiceRequestController> _logger;

        public ServiceRequestController(IServiceRequestService serviceRequestService, ILogger<ServiceRequestController> logger)
        {
            _serviceRequestService = serviceRequestService;
            _logger = logger;
        }

        /// <summary>
        /// GET: ServiceRequest
        /// Displays list of all service requests.
        /// </summary>
        public async Task<IActionResult> Index(string? status)
        {
            try
            {
                _logger.LogInformation("Loading service requests with status filter: {Status}", status ?? "All");
        
                // Get all requests first
                var requests = await _serviceRequestService.GetAllRequestsAsync();
        
                // Filter by status if provided
                if (!string.IsNullOrEmpty(status))
                {
                    // Filter the list based on status
                    requests = requests.Where(r => 
                        r.Status?.Equals(status, StringComparison.OrdinalIgnoreCase) == true
                    ).ToList();
            
                    _logger.LogInformation("Filtered to {Count} requests with status: {Status}", 
                        requests.Count, status);
                }
        
                // Pass the current filter to the view for highlighting active filter
                ViewBag.CurrentFilter = status;
        
                return View(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading service requests");
                ViewBag.ErrorMessage = "Unable to load service requests. Please try again later.";
                return View(new List<ServiceRequestListViewModel>());
            }
        }

        /// <summary>
        /// GET: ServiceRequest/PriorityQueue
        /// Displays requests sorted by priority using MinHeap.
        /// </summary>
        public async Task<IActionResult> PriorityQueue()
        {
            var model = await _serviceRequestService.GetRequestsByPriorityAsync();
            return View("Index", model); // Reuse Index view
        }

        /// <summary>
        /// GET: ServiceRequest/Details/5
        /// Displays details for a single request.
        /// Uses BST for lookup and Graph for dependencies.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var model = await _serviceRequestService.GetRequestDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}