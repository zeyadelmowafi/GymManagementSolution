using System.ComponentModel.DataAnnotations;
using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.BLL.ViewModels;

public class MemberViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class CreateMemberViewModel
{
    [Required, StringLength(100, MinimumLength = 2)] public string Name { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, Phone, RegularExpression(@"^(010|011|012|015)\d{8}$")] public string Phone { get; set; } = string.Empty;
    [Required, DataType(DataType.Date)] public DateOnly DateOfBirth { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required, Range(1, int.MaxValue)] public int BuildingNumber { get; set; }
    [Required] public string Street { get; set; } = string.Empty;
    [Required] public string City { get; set; } = string.Empty;
}
