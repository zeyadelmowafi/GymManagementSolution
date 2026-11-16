namespace GymManagementSolution.DAL.Entities;

public class Member : GymUser
{
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public HealthRecord? HealthRecord { get; set; }
}
