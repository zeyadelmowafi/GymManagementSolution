using GymManagementSolution.BLL.Results;
using GymManagementSolution.BLL.ViewModels;

namespace GymManagementSolution.BLL.Services;

public interface IMemberService
{
    Task<IReadOnlyList<MemberViewModel>> GetAllAsync(CancellationToken ct = default);
    Task<Result<int>> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default);
}
