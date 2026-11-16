using Hamac.Api.Models;

namespace Hamac.Api.Policies;

public interface ILeaveEntitlementPolicy
{
    string ContractType { get; }
    double ComputeEntitlement(Employee employee, DateTime today);
}
