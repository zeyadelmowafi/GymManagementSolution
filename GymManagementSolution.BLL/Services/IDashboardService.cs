using GymManagementSolution.BLL.ViewModels;

namespace GymManagementSolution.BLL.Services;

public interface IDashboardService { Task<DashboardViewModel> GetAsync(CancellationToken ct = default); }
