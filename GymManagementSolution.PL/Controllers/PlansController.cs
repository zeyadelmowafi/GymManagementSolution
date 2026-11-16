using GymManagementSolution.BLL.Services;
using GymManagementSolution.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSolution.PL.Controllers;

public class PlansController(IPlanService plans) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await plans.GetAllAsync(ct));
    public async Task<IActionResult> Details(int id, CancellationToken ct) { var r = await plans.GetDetailsAsync(id, ct); return r.IsSuccess ? View(r.Value) : NotFound(); }
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var r = await plans.GetForUpdateAsync(id, ct);
        if (!r.IsSuccess) { TempData["ErrorMessage"] = r.Error; return RedirectToAction(nameof(Index)); }
        return View(r.Value);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdatePlanViewModel vm, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(vm);
        var r = await plans.UpdateAsync(id, vm, ct);
        TempData[r.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = r.IsSuccess ? "Plan updated successfully." : r.Error;
        return r.IsSuccess ? RedirectToAction(nameof(Index)) : View(vm);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var r = await plans.ToggleActivationAsync(id, ct);
        TempData[r.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = r.IsSuccess ? "Plan status changed." : r.Error;
        return RedirectToAction(nameof(Index));
    }
}
