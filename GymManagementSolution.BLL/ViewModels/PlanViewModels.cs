using System.ComponentModel.DataAnnotations;

namespace GymManagementSolution.BLL.ViewModels;

public class PlanViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}

public class UpdatePlanViewModel
{
    public string PlanName { get; set; } = string.Empty;
    [Required, StringLength(500, MinimumLength = 5)] public string Description { get; set; } = string.Empty;
    [Range(1, 3650)] public int DurationDays { get; set; }
    [Range(typeof(decimal), "0.01", "1000000")] public decimal Price { get; set; }
}
