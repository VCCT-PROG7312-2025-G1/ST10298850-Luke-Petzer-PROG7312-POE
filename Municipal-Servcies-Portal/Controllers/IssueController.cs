// IssueController.cs
// Thin controller - delegates all business logic to IssueService.
// Follows MVVM pattern with ViewModels for input/output.
using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Services;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Controllers;

public class IssueController : Controller
{
    private readonly IIssueService _issueService;

    /// <summary>
    /// Constructor for dependency injection of IIssueService.
    /// </summary>
    public IssueController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    /// <summary>
    /// GET: Issue/Create
    /// Displays the issue creation form.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new IssueCreateViewModel());
    }

    /// <summary>
    /// POST: Issue/Create
    /// Handles form submission with validation and delegates to service.
    /// </summary>
    /// <param name="viewModel">The issue creation view model with user input.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IssueCreateViewModel viewModel)
    {
        // Return to form with validation errors if invalid
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        // Delegate creation to service (thick service handles all business logic)
        var issueId = await _issueService.CreateIssueAsync(viewModel);
        
        // Redirect to confirmation with the created issue ID
        return RedirectToAction("Confirmation", new { id = issueId });
    }

    /// <summary>
    /// GET: Issue/Confirmation/{id}
    /// Displays the confirmation page with submitted issue details.
    /// </summary>
    /// <param name="id">The ID of the created issue.</param>
    public async Task<IActionResult> Confirmation(int id)
    {
        // Delegate retrieval to service
        var issueViewModel = await _issueService.GetIssueByIdAsync(id);
        
        // If issue not found, redirect to home
        if (issueViewModel == null)
        {
            return RedirectToAction("Index", "Home");
        }

        // Pass ViewModel to view
        return View(issueViewModel);
    }
}