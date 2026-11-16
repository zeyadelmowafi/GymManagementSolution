using GymManagementSolution.DAL.Data;
using GymManagementSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSolution.DAL.Repositories;

public class SessionRepository(GymDbContext context) : GenericRepository<Session>(context), ISessionRepository
{
    public async Task<IReadOnlyList<Session>> GetAllWithDetailsAsync(CancellationToken ct = default) =>
        await Set.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).Include(x => x.Bookings).OrderBy(x => x.StartDate).ToListAsync(ct);
    public Task<Session?> GetWithDetailsAsync(int id, CancellationToken ct = default) =>
        Set.Include(x => x.Trainer).Include(x => x.Category).Include(x => x.Bookings).FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<bool> HasTrainerConflictAsync(int trainerId, DateTime start, DateTime end, int? excludeSessionId = null, CancellationToken ct = default) =>
        Set.AnyAsync(x => x.TrainerId == trainerId && (!excludeSessionId.HasValue || x.Id != excludeSessionId.Value) && x.StartDate < end && x.EndDate > start, ct);
    public Task<int> CountBookingsAsync(int sessionId, CancellationToken ct = default) => Context.Bookings.CountAsync(x => x.SessionId == sessionId, ct);
}
