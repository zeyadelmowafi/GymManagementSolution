namespace GymManagementSolution.DAL.Entities;

public class Session
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TrainerId { get; set; }
    public Trainer? Trainer { get; set; }
    public int CategoryId { get; set; }
    public SessionCategory? Category { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
