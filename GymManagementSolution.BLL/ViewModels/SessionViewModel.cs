using System.ComponentModel.DataAnnotations;

namespace GymManagementSolution.BLL.ViewModels;

public class SessionViewModel
{
    public int Id { get; set; }
    public string Description { get; set; } = default!;
    public int Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TrainerName { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public int TrainerId { get; set; }
    public int CategoryId { get; set; }
    public int AvailableSlots { get; set; }
    public string DateDisplay => $"{StartDate:MMM dd, yyyy}";
    public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";
    public TimeSpan Duration => EndDate - StartDate;
    public string DurationDisplay => $"{(int)Duration.TotalHours} Hours {Duration.Minutes} Minutes";
    public int BookedCount => Capacity - AvailableSlots;
    public string Status => StartDate > DateTime.Now ? "Upcoming" : StartDate <= DateTime.Now && EndDate >= DateTime.Now ? "Ongoing" : "Completed";
}

public class CreateSessionViewModel
{
    [Required(ErrorMessage = "Description is required"), StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
    public string Description { get; set; } = default!;
    [Required(ErrorMessage = "Capacity is required"), Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25")]
    public int Capacity { get; set; }
    [Required(ErrorMessage = "Start date is required"), Display(Name = "Start Date & Time"), DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End date is required"), Display(Name = "End Date & Time"), DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Trainer is required"), Display(Name = "Trainer"), Range(1, int.MaxValue)]
    public int TrainerId { get; set; }
    [Required(ErrorMessage = "Category is required"), Display(Name = "Category"), Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}

public class UpdateSessionViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Description is required"), StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
    public string Description { get; set; } = default!;
    [Required(ErrorMessage = "Start date is required"), Display(Name = "Start Date & Time"), DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "End date is required"), Display(Name = "End Date & Time"), DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Trainer is required"), Display(Name = "Trainer"), Range(1, int.MaxValue)]
    public int TrainerId { get; set; }
}
