using Hamac.Api.Models;

namespace Hamac.Api.Policies;

public sealed class PermanentContractPolicy : ILeaveEntitlementPolicy
{
    private const double AnnualDays = 25;

    public string ContractType => ContractTypes.Permanent;

    public double ComputeEntitlement(Employee employee, DateTime today) => AnnualDays;
}
