using GymManagementSolution.BLL.Services;
using GymManagementSolution.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSolution.PL.Controllers;

public class TrainersController(ITrainerService trainers) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await trainers.GetAllAsync(ct));
    public async Task<IActionResult> Create(CancellationToken ct) { ViewBag.Specialties = new SelectList(await trainers.GetSpecialtiesAsync(ct), "Value", "Text"); return View(); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) { ViewBag.Specialties = new SelectList(await trainers.GetSpecialtiesAsync(ct), "Value", "Text"); return View(model); }
        var result = await trainers.CreateAsync(model, ct);
        if (!result.IsSuccess) { ModelState.AddModelError(string.Empty, result.Error!); ViewBag.Specialties = new SelectList(await trainers.GetSpecialtiesAsync(ct), "Value", "Text"); return View(model); }
        TempData["SuccessMessage"] = "Trainer created successfully."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await trainers.GetForUpdateAsync(id, ct); if (!result.IsSuccess) return NotFound();
        ViewBag.Specialties = new SelectList(await trainers.GetSpecialtiesAsync(ct), "Value", "Text", result.Value!.Specialties); return View(result.Value);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TrainerToUpdateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) { ViewBag.Specialties = new SelectList(await trainers.GetSpecialtiesAsync(ct), "Value", "Text", model.Specialties); return View(model); }
        var result = await trainers.UpdateAsync(model, ct);
        if (!result.IsSuccess) { ModelState.AddModelError(string.Empty, result.Error!); ViewBag.Specialties = new SelectList(await trainers.GetSpecialtiesAsync(ct), "Value", "Text", model.Specialties); return View(model); }
        TempData["SuccessMessage"] = "Trainer updated successfully."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await trainers.GetForDeleteAsync(id, ct); if (!result.IsSuccess) return NotFound(); return View(result.Value);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
    {
        var result = await trainers.DeleteAsync(id, ct);
        if (!result.IsSuccess) { TempData["ErrorMessage"] = result.Error; return RedirectToAction(nameof(Index)); }
        TempData["SuccessMessage"] = "Trainer deleted successfully."; return RedirectToAction(nameof(Index));
    }
}
