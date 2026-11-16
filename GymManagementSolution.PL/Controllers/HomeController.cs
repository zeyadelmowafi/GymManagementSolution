using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymManagementSolution.BLL.Services;
using GymManagementSolution.PL.Models;

namespace GymManagementSolution.PL.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDashboardService _dashboard;

    public HomeController(ILogger<HomeController> logger, IDashboardService dashboard)
    {
        _logger = logger;
        _dashboard = dashboard;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        return View(await _dashboard.GetAsync(ct));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
