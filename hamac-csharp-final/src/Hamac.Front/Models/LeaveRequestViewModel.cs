using System.Text.Json.Serialization;

namespace Hamac.Front.Models;

public sealed class LeaveRequestViewModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [JsonPropertyName("workingDays")]
    public int Days { get; set; }

    public string Status { get; set; } = "";
}
