using AutoMapper;
using GymManagementSolution.BLL.Results;
using GymManagementSolution.BLL.ViewModels;
using GymManagementSolution.DAL.Entities;
using GymManagementSolution.DAL.Repositories;

namespace GymManagementSolution.BLL.Services.Classes;

public class MemberService(IUnitOfWork unitOfWork, IMapper mapper) : IMemberService
{
    public async Task<IReadOnlyList<MemberViewModel>> GetAllAsync(CancellationToken ct = default) => mapper.Map<IReadOnlyList<MemberViewModel>>(await unitOfWork.Members.GetAllAsync(ct));
    public async Task<Result<int>> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default)
    {
        var members = await unitOfWork.Members.GetAllAsync(ct); var trainers = await unitOfWork.Trainers.GetAllAsync(ct);
        if (members.Cast<GymUser>().Concat(trainers).Any(x => x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) || x.Phone == model.Phone))
            return Result<int>.Fail("Email or phone is already registered.", ResultCode.Conflict);
        var member = mapper.Map<Member>(model);
        await unitOfWork.Members.AddAsync(member, ct); await unitOfWork.SaveChangesAsync(ct);
        return Result<int>.Ok(member.Id);
    }
}
