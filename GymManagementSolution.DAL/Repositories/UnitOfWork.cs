using GymManagementSolution.DAL.Data;
using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.DAL.Repositories;

public class UnitOfWork(GymDbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (!_repositories.TryGetValue(type, out var repository))
            _repositories[type] = repository = new GenericRepository<TEntity>(context);
        return (IGenericRepository<TEntity>)repository;
    }
    private IGenericRepository<Member>? _members;
    private IGenericRepository<Trainer>? _trainers;
    private IGenericRepository<Plan>? _plans;
    private IGenericRepository<Membership>? _memberships;
    private ISessionRepository? _sessions;
    private IGenericRepository<SessionCategory>? _categories;
    private IGenericRepository<Booking>? _bookings;
    private IGenericRepository<HealthRecord>? _healthRecords;
    public IGenericRepository<Member> Members => _members ??= new GenericRepository<Member>(context);
    public IGenericRepository<Trainer> Trainers => _trainers ??= new GenericRepository<Trainer>(context);
    public IGenericRepository<Plan> Plans => _plans ??= new GenericRepository<Plan>(context);
    public IGenericRepository<Membership> Memberships => _memberships ??= new GenericRepository<Membership>(context);
    public ISessionRepository Sessions => _sessions ??= new SessionRepository(context);
    public IGenericRepository<SessionCategory> SessionCategories => _categories ??= new GenericRepository<SessionCategory>(context);
    public IGenericRepository<Booking> Bookings => _bookings ??= new GenericRepository<Booking>(context);
    public IGenericRepository<HealthRecord> HealthRecords => _healthRecords ??= new GenericRepository<HealthRecord>(context);
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
    public ValueTask DisposeAsync() => context.DisposeAsync();
}
