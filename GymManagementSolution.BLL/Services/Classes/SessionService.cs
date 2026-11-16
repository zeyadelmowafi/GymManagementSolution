using AutoMapper;
using GymManagementSolution.BLL.Results;
using GymManagementSolution.BLL.ViewModels;
using GymManagementSolution.DAL.Entities;
using GymManagementSolution.DAL.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSolution.BLL.Services.Classes;

public class SessionService(IUnitOfWork unitOfWork, IMapper mapper) : ISessionService
{
    public async Task<IReadOnlyList<SessionViewModel>> GetAllAsync(CancellationToken ct = default) =>
        mapper.Map<IReadOnlyList<SessionViewModel>>(await unitOfWork.Sessions.GetAllWithDetailsAsync(ct));

    public async Task<Result<SessionViewModel>> GetDetailsAsync(int id, CancellationToken ct = default)
    {
        var session = await unitOfWork.Sessions.GetWithDetailsAsync(id, ct);
        return session is null ? Result<SessionViewModel>.Fail("Session not found.", ResultCode.NotFound) : Result<SessionViewModel>.Ok(mapper.Map<SessionViewModel>(session));
    }

    public async Task<IReadOnlyList<SelectListItem>> GetCategoriesAsync(CancellationToken ct = default) =>
        (await unitOfWork.SessionCategories.GetAllAsync(ct)).OrderBy(x => x.Name).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

    public async Task<IReadOnlyList<SelectListItem>> GetTrainersAsync(int categoryId, CancellationToken ct = default)
    {
        var category = (await unitOfWork.SessionCategories.GetAllAsync(ct)).FirstOrDefault(x => x.Id == categoryId);
        var trainers = await unitOfWork.Trainers.GetAllAsync(ct);
        if (category is not null) trainers = trainers.Where(x => x.Specialties.Equals(category.Name, StringComparison.OrdinalIgnoreCase)).ToList();
        return trainers.OrderBy(x => x.Name).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
    }

    public async Task<Result<int>> CreateAsync(CreateSessionViewModel model, CancellationToken ct = default)
    {
        var failure = await ValidateAsync(model.CategoryId, model.TrainerId, model.StartDate, model.EndDate, model.Capacity, null, ct);
        if (failure is not null) return Result<int>.Fail(failure);
        var session = mapper.Map<Session>(model);
        await unitOfWork.Sessions.AddAsync(session, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result<int>.Ok(session.Id);
    }

    public async Task<Result<int>> UpdateAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
    {
        var session = await unitOfWork.Sessions.GetWithDetailsAsync(id, ct);
        if (session is null) return Result<int>.Fail("Session not found.", ResultCode.NotFound);
        if (session.StartDate <= DateTime.Now) return Result<int>.Fail("Ongoing or completed sessions cannot be edited.", ResultCode.Forbidden);
        var failure = await ValidateAsync(session.CategoryId, model.TrainerId, model.StartDate, model.EndDate, session.Capacity, id, ct);
        if (failure is not null) return Result<int>.Fail(failure);
        session.Description = model.Description.Trim(); session.StartDate = model.StartDate; session.EndDate = model.EndDate; session.TrainerId = model.TrainerId;
        unitOfWork.Sessions.Update(session);
        await unitOfWork.SaveChangesAsync(ct);
        return Result<int>.Ok(session.Id);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct = default)
    {
        var session = await unitOfWork.Sessions.GetWithDetailsAsync(id, ct);
        if (session is null) return Result<bool>.Fail("Session not found.", ResultCode.NotFound);
        if (session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now) return Result<bool>.Fail("An ongoing session cannot be deleted.", ResultCode.Forbidden);
        unitOfWork.Sessions.Remove(session);
        await unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Ok(true);
    }

    private async Task<string?> ValidateAsync(int categoryId, int trainerId, DateTime start, DateTime end, int capacity, int? excludeId, CancellationToken ct)
    {
        if (start >= end) return "End date and time must be after the start.";
        if (start <= DateTime.Now) return "A session must start in the future.";
        if (capacity < 1 || capacity > 25) return "Capacity must be between 1 and 25.";
        var categories = await unitOfWork.SessionCategories.GetAllAsync(ct);
        var category = categories.FirstOrDefault(x => x.Id == categoryId);
        if (category is null) return "Select a valid category.";
        var trainer = (await unitOfWork.Trainers.GetAllAsync(ct)).FirstOrDefault(x => x.Id == trainerId);
        if (trainer is null) return "Select a valid trainer.";
        if (!trainer.Specialties.Equals(category.Name, StringComparison.OrdinalIgnoreCase)) return "The trainer specialty must match the session category.";
        if (await unitOfWork.Sessions.HasTrainerConflictAsync(trainerId, start, end, excludeId, ct)) return "The trainer is already scheduled during this time slot.";
        return null;
    }
}
