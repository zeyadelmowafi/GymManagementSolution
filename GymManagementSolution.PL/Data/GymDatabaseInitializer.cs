using System.Text.Json;
using GymManagementSolution.DAL.Data;
using GymManagementSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSolution.PL.Data;

public static class GymDatabaseInitializer
{
    private static readonly string[] Categories = ["Boxing", "CrossFit", "Yoga", "Strength Training", "Pilates", "Cardio"];

    public static async Task InitializeAsync(IServiceProvider services, string contentRoot)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        await db.Database.EnsureCreatedAsync();
        if (!await db.SessionCategories.AnyAsync())
        {
            db.SessionCategories.AddRange(Categories.Select(name => new SessionCategory { Name = name }));
            await db.SaveChangesAsync();
        }
        if (await db.Plans.AnyAsync()) return;
        var path = Path.Combine(contentRoot, "plans.json");
        if (!File.Exists(path)) return;
        var json = await File.ReadAllTextAsync(path);
        var plans = JsonSerializer.Deserialize<List<Plan>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (plans is { Count: > 0 }) { db.Plans.AddRange(plans); await db.SaveChangesAsync(); }
    }
}
