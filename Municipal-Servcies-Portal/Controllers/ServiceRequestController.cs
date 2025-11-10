using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Services;

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
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("ServiceRequest/Index called");
                var model = await _serviceRequestService.GetAllRequestsAsync();
                _logger.LogInformation($"Retrieved {model.Count} service requests");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading service requests");
                ViewBag.ErrorMessage = $"Error loading service requests: {ex.Message}";
                return View(new List<Municipal_Servcies_Portal.ViewModels.ServiceRequestListViewModel>());
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