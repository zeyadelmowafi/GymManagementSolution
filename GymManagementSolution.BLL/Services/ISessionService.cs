using GymManagementSolution.BLL.Results;
using GymManagementSolution.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSolution.BLL.Services;

public interface ISessionService
{
    Task<IReadOnlyList<SessionViewModel>> GetAllAsync(CancellationToken ct = default);
    Task<Result<SessionViewModel>> GetDetailsAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SelectListItem>> GetCategoriesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SelectListItem>> GetTrainersAsync(int categoryId, CancellationToken ct = default);
    Task<Result<int>> CreateAsync(CreateSessionViewModel model, CancellationToken ct = default);
    Task<Result<int>> UpdateAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct = default);
}
