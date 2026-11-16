using Hamac.Api.Models;

namespace Hamac.Api.Dtos;

public sealed record LeaveRequestDto(
    int Id,
    int EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    int WorkingDays,
    string Status)
{
    public static LeaveRequestDto From(LeaveRequest request) => new(
        request.Id,
        request.EmployeeId,
        request.StartDate,
        request.EndDate,
        request.WorkingDays,
        request.Status);
}
