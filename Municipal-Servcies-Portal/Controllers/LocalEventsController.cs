// LocalEventsController.cs
// Thin controller - delegates all business logic to LocalEventsService
// Handles request routing and view selection only
using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Services;

namespace Municipal_Servcies_Portal.Controllers
{
    public class LocalEventsController : Controller
    {
        private readonly ILocalEventsService _localEventsService;

        public LocalEventsController(ILocalEventsService localEventsService)
        {
            _localEventsService = localEventsService;
        }

        /// <summary>
        /// GET: LocalEvents/Index
        /// Displays events page with optional search parameters from query string
        /// </summary>
        public async Task<IActionResult> Index(string? searchName, string? category, DateTime? date)
        {
            // Delegate all logic to service - thick service handles filtering, data retrieval, etc.
            var viewModel = await _localEventsService.GetEventsPageViewModelAsync(
                searchName, 
                category, 
                date);

            return View(viewModel);
        }

        /// <summary>
        /// GET: LocalEvents/Details/{id}
        /// Displays detailed view of a single event
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var eventDetails = await _localEventsService.GetEventByIdAsync(id);

            if (eventDetails == null)
            {
                return NotFound();
            }

            return View(eventDetails);
        }
    }
}
