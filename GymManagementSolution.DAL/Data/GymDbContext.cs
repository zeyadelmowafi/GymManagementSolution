using GymManagementSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSolution.DAL.Data;

public class GymDbContext(DbContextOptions<GymDbContext> options) : DbContext(options)
{
    public DbSet<GymUser> Users => Set<GymUser>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionCategory> SessionCategories => Set<SessionCategory>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GymUser>().HasDiscriminator<string>("UserType").HasValue<Member>("Member").HasValue<Trainer>("Trainer");
        modelBuilder.Entity<GymUser>().OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(120);
            address.Property(a => a.City).HasMaxLength(100);
        });
        modelBuilder.Entity<GymUser>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<GymUser>().HasIndex(x => x.Phone).IsUnique();
        modelBuilder.Entity<Plan>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Plan>().Property(x => x.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Plan>().Property(x => x.Name).HasMaxLength(80).IsRequired();
        modelBuilder.Entity<Session>().Property(x => x.Description).HasMaxLength(500).IsRequired();
        modelBuilder.Entity<Session>().HasOne(x => x.Trainer).WithMany(x => x.Sessions).HasForeignKey(x => x.TrainerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Session>().HasOne(x => x.Category).WithMany(x => x.Sessions).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<SessionCategory>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Booking>().HasIndex(x => new { x.MemberId, x.SessionId }).IsUnique();
        modelBuilder.Entity<Booking>().HasOne(x => x.Session).WithMany(x => x.Bookings).HasForeignKey(x => x.SessionId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<HealthRecord>().HasIndex(x => x.MemberId).IsUnique();
        modelBuilder.Entity<Member>().HasOne(x => x.HealthRecord).WithOne(x => x.Member)
            .HasForeignKey<HealthRecord>(x => x.MemberId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Membership>().HasOne(x => x.Member).WithMany(x => x.Memberships).HasForeignKey(x => x.MemberId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Membership>().HasOne(x => x.Plan).WithMany(x => x.Memberships).HasForeignKey(x => x.PlanId).OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(modelBuilder);
    }
}
