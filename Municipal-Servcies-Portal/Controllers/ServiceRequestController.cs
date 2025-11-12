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
        /// Displays list of all service requests with optional search/filter.
        /// All business logic handled in service layer.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Loading all service requests");
                var model = await _serviceRequestService.SearchRequestsAsync(null, null);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading service requests");
                ViewBag.ErrorMessage = "Unable to load service requests. Please try again later.";
                return View(new ServiceRequestSearchViewModel());
            }
        }
        
        /// <summary>
        /// GET: ServiceRequest/Search
        /// Handles search and filter requests.
        /// All business logic handled in service layer.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Search(string? searchTerm, string? category)
        {
            try
            {
                _logger.LogInformation("Search: Term='{SearchTerm}', Category='{Category}'", searchTerm, category);
                var model = await _serviceRequestService.SearchRequestsAsync(searchTerm, category);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching service requests");
                ViewBag.ErrorMessage = "Search failed. Please try again.";
                return View("Index", new ServiceRequestSearchViewModel());
            }
        }
        
        /// <summary>
        /// GET: ServiceRequest/ClearSearch
        /// Clears search filters and returns to full list.
        /// </summary>
        [HttpGet]
        public IActionResult ClearSearch()
        {
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: ServiceRequest/PriorityQueue
        /// Displays requests sorted by priority using MinHeap.
        /// All business logic handled in service layer.
        /// </summary>
        public async Task<IActionResult> PriorityQueue()
        {
            try
            {
                _logger.LogInformation("Loading priority queue");
                var model = await _serviceRequestService.GetRequestsByPriorityAsync();
                
                // Create search view model wrapper for consistency with Index view
                var searchViewModel = new ServiceRequestSearchViewModel
                {
                    Results = model,
                    TotalResults = model.Count,
                    AvailableCategories = await _serviceRequestService.GetAllCategoriesAsync()
                };
                
                return View("Index", searchViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading priority queue");
                ViewBag.ErrorMessage = "Unable to load priority queue. Please try again.";
                return View("Index", new ServiceRequestSearchViewModel());
            }
        }

        /// <summary>
        /// GET: ServiceRequest/Details/5
        /// Displays details for a single request.
        /// Uses BST for lookup and Graph for dependencies.
        /// All business logic handled in service layer.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation("Loading details for request {Id}", id);
                var model = await _serviceRequestService.GetRequestDetailsAsync(id);

                if (model == null)
                {
                    _logger.LogWarning("Request {Id} not found", id);
                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading request details for {Id}", id);
                ViewBag.ErrorMessage = "Unable to load request details. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

