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

    public ApprovalResult Approve(int requestId)
    {
        // Filtrage en mémoire conservé : il sera corrigé au chapitre 4.
        var request = _db.LeaveRequests.ToList().FirstOrDefault(r => r.Id == requestId);
        if (request is null) return ApprovalResult.NotFound;

        var employee = _db.Employees.Find(request.EmployeeId)!;

        if (ComputeBalance(employee) < request.WorkingDays)
        {
            request.Status = "Refused";
            _db.SaveChanges();
            return ApprovalResult.InsufficientBalance;
        }

        request.Status = "Approved";
        employee.DaysTaken += request.WorkingDays;
        _db.SaveChanges();

        _notifier.Notify(employee, "Votre demande de congés est approuvée.");
        return ApprovalResult.Approved;
    }

    private double ComputeBalance(Employee employee) =>
        _policies.For(employee.ContractType).ComputeEntitlement(employee, DateTime.Today)
        - employee.DaysTaken;
}
