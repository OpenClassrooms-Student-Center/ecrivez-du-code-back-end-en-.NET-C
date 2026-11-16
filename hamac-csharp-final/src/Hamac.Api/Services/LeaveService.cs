using Hamac.Api.Data;
using Hamac.Api.Models;
using Hamac.Api.Policies;

namespace Hamac.Api.Services;

public sealed class LeaveService
{
    private readonly HamacDbContext _db;
    private readonly LeavePolicyResolver _policies;
    private readonly ILeaveNotifier _notifier;

    public LeaveService(HamacDbContext db, LeavePolicyResolver policies, ILeaveNotifier notifier)
    {
        _db = db;
        _policies = policies;
        _notifier = notifier;
    }

    public double? GetBalance(int employeeId)
    {
        var employee = _db.Employees.Find(employeeId);
        return employee is null ? null : ComputeBalance(employee);
    }

    public IReadOnlyList<LeaveRequest> GetRequests(string? status) =>
        _db.LeaveRequests
            .Where(r => status == null || r.Status == status)
            .OrderBy(r => r.StartDate)
            .ToList();

    public LeaveRequest? GetRequest(int requestId) => _db.LeaveRequests.Find(requestId);

    public LeaveRequest? Create(int employeeId, DateTime startDate, DateTime endDate)
    {
        if (_db.Employees.Find(employeeId) is null) return null;

        var request = new LeaveRequest
        {
            EmployeeId = employeeId,
            StartDate = startDate,
            EndDate = endDate,
            WorkingDays = CountWorkingDays(startDate, endDate),
            Status = LeaveStatuses.Pending,
        };

        _db.LeaveRequests.Add(request);
        _db.SaveChanges();
        return request;
    }

    public ApprovalResult Approve(int requestId)
    {
        var request = _db.LeaveRequests.Find(requestId);
        if (request is null) return ApprovalResult.NotFound;
        if (request.Status != LeaveStatuses.Pending) return ApprovalResult.NotPending;

        var employee = _db.Employees.Find(request.EmployeeId)!;

        if (ComputeBalance(employee) < request.WorkingDays)
        {
            request.Status = LeaveStatuses.Refused;
            _db.SaveChanges();
            return ApprovalResult.InsufficientBalance;
        }

        request.Status = LeaveStatuses.Approved;
        employee.DaysTaken += request.WorkingDays;
        _db.SaveChanges();

        _notifier.Notify(employee, "Votre demande de congés est approuvée.");
        return ApprovalResult.Approved;
    }

    public RefusalResult Refuse(int requestId)
    {
        var request = _db.LeaveRequests.Find(requestId);
        if (request is null) return RefusalResult.NotFound;
        if (request.Status != LeaveStatuses.Pending) return RefusalResult.NotPending;

        request.Status = LeaveStatuses.Refused;
        _db.SaveChanges();
        return RefusalResult.Refused;
    }

    private double ComputeBalance(Employee employee) =>
        _policies.For(employee.ContractType).ComputeEntitlement(employee, DateTime.Today)
        - employee.DaysTaken;

    private static int CountWorkingDays(DateTime startDate, DateTime endDate) =>
        Enumerable.Range(0, (endDate.Date - startDate.Date).Days + 1)
            .Select(offset => startDate.Date.AddDays(offset))
            .Count(day => day.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday);
}
