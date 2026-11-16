using System.ComponentModel.DataAnnotations;
using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.BLL.ViewModels;

public class TrainerViewModel
{
    public int Id { get; set; }
    public string? Photo { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string DateOfBirth { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Specialties { get; set; } = default!;
}

public class CreateTrainerViewModel
{
    [Required, StringLength(100, MinimumLength = 2)] public string Name { get; set; } = default!;
    [Required, EmailAddress(ErrorMessage = "Invalid email format")] public string Email { get; set; } = default!;
    [Required(ErrorMessage = "Phone Number Is Required"), Phone(ErrorMessage = "Invalid phone number"), RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
    public string Phone { get; set; } = default!;
    [Required, DataType(DataType.Date)] public DateOnly DateOfBirth { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required, Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than 0")] public int BuildingNumber { get; set; }
    [Required, StringLength(120, MinimumLength = 2)] public string Street { get; set; } = default!;
    [Required, StringLength(100, MinimumLength = 2)] public string City { get; set; } = default!;
    [Required, Display(Name = "Specialty")] public string Specialties { get; set; } = default!;
}

public class TrainerToUpdateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    [Required, EmailAddress] public string Email { get; set; } = default!;
    [Required, Phone, RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")] public string Phone { get; set; } = default!;
    [Required, DataType(DataType.Date)] public DateOnly DateOfBirth { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required, Range(1, int.MaxValue)] public int BuildingNumber { get; set; }
    [Required] public string Street { get; set; } = default!;
    [Required] public string City { get; set; } = default!;
    [Required, Display(Name = "Specialty")] public string Specialties { get; set; } = default!;
}

public class TrainerDeleteViewModel { public int Id { get; set; } public string Name { get; set; } = string.Empty; public string? Error { get; set; } }
