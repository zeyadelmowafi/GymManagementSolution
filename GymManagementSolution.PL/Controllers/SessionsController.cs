using GymManagementSolution.BLL.Services;
using GymManagementSolution.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSolution.PL.Controllers;

public class SessionsController(ISessionService sessions) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await sessions.GetAllAsync(ct));
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var result = await sessions.GetDetailsAsync(id, ct);
        return result.IsSuccess ? View(result.Value) : NotFound();
    }
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        await LoadOptions(null, ct);
        return View(new CreateSessionViewModel { StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(1).AddHours(1) });
    }
    [HttpGet]
    public async Task<IActionResult> TrainersForCategory(int categoryId, CancellationToken ct) => Json(await sessions.GetTrainersAsync(categoryId, ct));
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) { await LoadOptions(model.CategoryId, ct); return View(model); }
        var result = await sessions.CreateAsync(model, ct);
        if (!result.IsSuccess) { ModelState.AddModelError(string.Empty, result.Error!); await LoadOptions(model.CategoryId, ct); return View(model); }
        TempData["SuccessMessage"] = "Session created successfully.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await sessions.GetDetailsAsync(id, ct);
        if (!result.IsSuccess) return NotFound();
        var session = result.Value!;
        if (session.Status != "Upcoming") { TempData["ErrorMessage"] = "Only upcoming sessions can be edited."; return RedirectToAction(nameof(Index)); }
        var model = new UpdateSessionViewModel { Id = session.Id, Description = session.Description, StartDate = session.StartDate, EndDate = session.EndDate, TrainerId = session.TrainerId };
        await LoadOptions(session.CategoryId, ct);
        ViewBag.CategoryName = session.CategoryName; ViewBag.Capacity = session.Capacity;
        return View(model);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
    {
        if (id != model.Id) return BadRequest();
        var existing = await sessions.GetDetailsAsync(id, ct);
        if (!existing.IsSuccess) return NotFound();
        if (!ModelState.IsValid) { await LoadOptions(existing.Value!.CategoryId, ct); ViewBag.CategoryName = existing.Value.CategoryName; ViewBag.Capacity = existing.Value.Capacity; return View(model); }
        var result = await sessions.UpdateAsync(id, model, ct);
        if (!result.IsSuccess) { ModelState.AddModelError(string.Empty, result.Error!); await LoadOptions(existing.Value!.CategoryId, ct); ViewBag.CategoryName = existing.Value.CategoryName; ViewBag.Capacity = existing.Value.Capacity; return View(model); }
        TempData["SuccessMessage"] = "Session updated successfully.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await sessions.GetDetailsAsync(id, ct);
        return result.IsSuccess ? View(result.Value) : NotFound();
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
    {
        var result = await sessions.DeleteAsync(id, ct);
        if (!result.IsSuccess) { TempData["ErrorMessage"] = result.Error; return RedirectToAction(nameof(Index)); }
        TempData["SuccessMessage"] = "Session deleted.";
        return RedirectToAction(nameof(Index));
    }
    private async Task LoadOptions(int? categoryId, CancellationToken ct)
    {
        ViewBag.Categories = new SelectList(await sessions.GetCategoriesAsync(ct), "Value", "Text", categoryId);
        ViewBag.Trainers = new SelectList(await sessions.GetTrainersAsync(categoryId ?? 0, ct), "Value", "Text");
    }
}
