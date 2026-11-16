namespace GymManagementSolution.DAL.Entities;

public class Trainer : GymUser
{
    public string Specialties { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
