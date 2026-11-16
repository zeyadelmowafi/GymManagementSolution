namespace GymManagementSolution.DAL.Entities;

public class HealthRecord
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public Member? Member { get; set; }
    public string? Notes { get; set; }
    public string? MedicalConditions { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
