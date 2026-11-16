using GymManagementSolution.BLL.Results;
using GymManagementSolution.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSolution.BLL.Services;

public interface ITrainerService
{
    Task<IReadOnlyList<TrainerViewModel>> GetAllAsync(CancellationToken ct = default);
    Task<Result<TrainerToUpdateViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SelectListItem>> GetSpecialtiesAsync(CancellationToken ct = default);
    Task<Result<int>> CreateAsync(CreateTrainerViewModel model, CancellationToken ct = default);
    Task<Result<bool>> UpdateAsync(TrainerToUpdateViewModel model, CancellationToken ct = default);
    Task<Result<TrainerDeleteViewModel>> GetForDeleteAsync(int id, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(int id, CancellationToken ct = default);
}
