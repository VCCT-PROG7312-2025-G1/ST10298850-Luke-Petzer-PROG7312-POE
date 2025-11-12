using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Municipal_Servcies_Portal.Models;
using Municipal_Servcies_Portal.Repositories;
using Municipal_Servcies_Portal.ViewModels;

namespace Municipal_Servcies_Portal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IIssueRepository _issueRepository;

    public HomeController(ILogger<HomeController> logger, IIssueRepository issueRepository)
    {
        _logger = logger;
        _issueRepository = issueRepository;
    }

    public async Task<IActionResult> Index()
    {
        // Fetch dashboard statistics from database
        var stats = new DashboardStatsViewModel
        {
            IssuesResolved = await _issueRepository.GetResolvedIssuesCountAsync(),
            ActiveRequests = await _issueRepository.GetActiveIssuesCountAsync(),
            AverageResponseTime = FormatResponseTime(await _issueRepository.GetAverageResponseTimeInHoursAsync())
        };

        return View(stats);
    }

    private string FormatResponseTime(double hours)
    {
        if (hours == 0)
            return "N/A";

        // If less than 48 hours, show in hours
        if (hours < 48)
            return $"{Math.Round(hours)}h";

        // Otherwise show in days
        var days = Math.Round(hours / 24, 1);
        return $"{days}d";
    }

    public IActionResult ComingSoon()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}