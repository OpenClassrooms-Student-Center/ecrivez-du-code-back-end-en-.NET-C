using Hamac.Api.Models;

namespace Hamac.Api.Policies;

public interface ILeaveEntitlementPolicy
{
    double ComputeEntitlement(Employee employee, DateTime today);
}
