using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.DAL.Repositories;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<IReadOnlyList<Session>> GetAllWithDetailsAsync(CancellationToken ct = default);
    Task<Session?> GetWithDetailsAsync(int id, CancellationToken ct = default);
    Task<bool> HasTrainerConflictAsync(int trainerId, DateTime start, DateTime end, int? excludeSessionId = null, CancellationToken ct = default);
    Task<int> CountBookingsAsync(int sessionId, CancellationToken ct = default);
}
