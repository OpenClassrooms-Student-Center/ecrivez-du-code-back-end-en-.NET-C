using Hamac.Api.Models;

namespace Hamac.Api.Policies;

public sealed class ApprenticeshipPolicy : ILeaveEntitlementPolicy
{
    private const double AnnualDays = 25;
    private const double ExamPreparationDays = 5;

    public string ContractType => ContractTypes.Apprenticeship;

    public double ComputeEntitlement(Employee employee, DateTime today) =>
        AnnualDays + ExamPreparationDays;
}
