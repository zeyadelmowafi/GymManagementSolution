using GymManagementSolution.BLL.Services;
using GymManagementSolution.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSolution.PL.Controllers;

public class MembersController(IMemberService members) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await members.GetAllAsync(ct));
    public IActionResult Create() => View(new CreateMemberViewModel());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await members.CreateAsync(model, ct);
        if (!result.IsSuccess) { ModelState.AddModelError(string.Empty, result.Error!); return View(model); }
        TempData["SuccessMessage"] = "Member added."; return RedirectToAction(nameof(Index));
    }
}
