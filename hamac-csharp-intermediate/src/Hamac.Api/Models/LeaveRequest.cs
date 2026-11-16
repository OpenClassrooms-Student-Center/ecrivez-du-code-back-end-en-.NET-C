namespace Hamac.Api.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int WorkingDays { get; set; }
    public string Status { get; set; } = "Pending";
}
