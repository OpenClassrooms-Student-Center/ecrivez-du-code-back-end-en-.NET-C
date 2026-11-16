using Hamac.Api.Models;

namespace Hamac.Api.Policies;

public sealed class InternshipPolicy : ILeaveEntitlementPolicy
{
    private const double DaysPerFullMonth = 2;

    public string ContractType => ContractTypes.Internship;

    public double ComputeEntitlement(Employee employee, DateTime today)
    {
        var fullMonths = Math.Floor((today - employee.HireDate).TotalDays / 30);
        return Math.Max(0, fullMonths) * DaysPerFullMonth;
    }
}
