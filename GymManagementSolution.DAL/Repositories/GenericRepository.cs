using System.Linq.Expressions;
using GymManagementSolution.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSolution.DAL.Repositories;

public class GenericRepository<T>(GymDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly GymDbContext Context = context;
    protected readonly DbSet<T> Set = context.Set<T>();
    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) => await Set.FindAsync([id], ct);
    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default) => await Set.AsNoTracking().ToListAsync(ct);
    public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => Set.AnyAsync(predicate, ct);
    public virtual async Task AddAsync(T entity, CancellationToken ct = default) => await Set.AddAsync(entity, ct);
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) => await Set.AddRangeAsync(entities, ct);
    public virtual void Update(T entity) => Set.Update(entity);
    public virtual void Remove(T entity) => Set.Remove(entity);
}
