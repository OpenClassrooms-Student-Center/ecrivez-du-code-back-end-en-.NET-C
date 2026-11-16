using Hamac.Api.Models;

namespace Hamac.Api.Policies;

public sealed class FixedTermContractPolicy : ILeaveEntitlementPolicy
{
    private const double DaysPerFullMonth = 2.08;

    public double ComputeEntitlement(Employee employee, DateTime today)
    {
        var fullMonths = Math.Floor((today - employee.HireDate).TotalDays / 30);
        return fullMonths * DaysPerFullMonth;
    }
}
