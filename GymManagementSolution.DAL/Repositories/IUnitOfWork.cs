using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.DAL.Repositories;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    IGenericRepository<Member> Members { get; }
    IGenericRepository<Trainer> Trainers { get; }
    IGenericRepository<Plan> Plans { get; }
    IGenericRepository<Membership> Memberships { get; }
    ISessionRepository Sessions { get; }
    IGenericRepository<SessionCategory> SessionCategories { get; }
    IGenericRepository<Booking> Bookings { get; }
    IGenericRepository<HealthRecord> HealthRecords { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
