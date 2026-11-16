using AutoMapper;
using GymManagementSolution.BLL.Results;
using GymManagementSolution.BLL.ViewModels;
using GymManagementSolution.DAL.Entities;
using GymManagementSolution.DAL.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSolution.BLL.Services.Classes;

public class TrainerService(IUnitOfWork unitOfWork, IMapper mapper) : ITrainerService
{
    private static readonly string[] Specialties = ["Boxing", "CrossFit", "Yoga", "Strength Training", "Pilates", "Cardio"];
    public async Task<IReadOnlyList<TrainerViewModel>> GetAllAsync(CancellationToken ct = default) => mapper.Map<IReadOnlyList<TrainerViewModel>>(await unitOfWork.Trainers.GetAllAsync(ct));
    public async Task<Result<TrainerToUpdateViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.Trainers.GetByIdAsync(id, ct);
        return trainer is null ? Result<TrainerToUpdateViewModel>.Fail("Trainer not found.", ResultCode.NotFound) : Result<TrainerToUpdateViewModel>.Ok(mapper.Map<TrainerToUpdateViewModel>(trainer));
    }
    public Task<IReadOnlyList<SelectListItem>> GetSpecialtiesAsync(CancellationToken ct = default) => Task.FromResult<IReadOnlyList<SelectListItem>>(Specialties.Select(x => new SelectListItem(x, x)).ToList());
    public async Task<Result<int>> CreateAsync(CreateTrainerViewModel model, CancellationToken ct = default)
    {
        if (await DuplicateAsync(model.Email, model.Phone, null, ct)) return Result<int>.Fail("Email or phone is already registered.", ResultCode.Conflict);
        if (!Specialties.Contains(model.Specialties, StringComparer.OrdinalIgnoreCase)) return Result<int>.Fail("Choose a specialty from the list.");
        var trainer = mapper.Map<Trainer>(model); trainer.Email = model.Email.Trim(); trainer.Phone = model.Phone.Trim();
        await unitOfWork.Trainers.AddAsync(trainer, ct); await unitOfWork.SaveChangesAsync(ct);
        return Result<int>.Ok(trainer.Id);
    }
    public async Task<Result<bool>> UpdateAsync(TrainerToUpdateViewModel model, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.Trainers.GetByIdAsync(model.Id, ct);
        if (trainer is null) return Result<bool>.Fail("Trainer not found.", ResultCode.NotFound);
        if (await DuplicateAsync(model.Email, model.Phone, model.Id, ct)) return Result<bool>.Fail("Email or phone is already used by another trainer.", ResultCode.Conflict);
        if (!Specialties.Contains(model.Specialties, StringComparer.OrdinalIgnoreCase)) return Result<bool>.Fail("Choose a specialty from the list.");
        mapper.Map(model, trainer); trainer.Email = model.Email.Trim(); trainer.Phone = model.Phone.Trim(); trainer.UpdatedAt = DateTime.UtcNow;
        unitOfWork.Trainers.Update(trainer); await unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Ok(true);
    }
    public async Task<Result<TrainerDeleteViewModel>> GetForDeleteAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.Trainers.GetByIdAsync(id, ct);
        return trainer is null ? Result<TrainerDeleteViewModel>.Fail("Trainer not found.", ResultCode.NotFound) : Result<TrainerDeleteViewModel>.Ok(new TrainerDeleteViewModel { Id = id, Name = trainer.Name });
    }
    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var trainer = await unitOfWork.Trainers.GetByIdAsync(id, ct);
        if (trainer is null) return Result<bool>.Fail("Trainer not found.", ResultCode.NotFound);
        var scheduled = (await unitOfWork.Sessions.GetAllWithDetailsAsync(ct)).Any(x => x.TrainerId == id && x.EndDate >= DateTime.Now);
        if (scheduled) return Result<bool>.Fail("This trainer has a scheduled session and cannot be deleted.", ResultCode.Conflict);
        unitOfWork.Trainers.Remove(trainer); await unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Ok(true);
    }
    private async Task<bool> DuplicateAsync(string email, string phone, int? excludeId, CancellationToken ct)
    {
        var users = (await unitOfWork.Trainers.GetAllAsync(ct)).Cast<GymUser>().Concat((await unitOfWork.Members.GetAllAsync(ct)).Cast<GymUser>());
        return users.Any(x => x.Id != excludeId && (x.Email.Equals(email, StringComparison.OrdinalIgnoreCase) || x.Phone == phone));
    }
}
