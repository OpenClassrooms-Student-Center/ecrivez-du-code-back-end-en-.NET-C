using Hamac.Api.Models;
using Hamac.Api.Policies;

namespace Hamac.Tests;

public class LeavePolicyTests
{
    [Fact]
    public void Fixed_term_policy_counts_only_full_months()
    {
        var policy = new FixedTermContractPolicy();
        var employee = new Employee { HireDate = new DateTime(2026, 3, 15) };

        var entitlement = policy.ComputeEntitlement(employee, new DateTime(2026, 10, 20));

        Assert.Equal(14.56, entitlement, precision: 2);
    }

    [Fact]
    public void Internship_policy_grants_two_days_per_full_month()
    {
        var policy = new InternshipPolicy();
        var employee = new Employee { HireDate = new DateTime(2026, 3, 15) };

        var entitlement = policy.ComputeEntitlement(employee, new DateTime(2026, 10, 20));

        Assert.Equal(14.0, entitlement);
    }

    [Fact]
    public void Internship_policy_grants_nothing_before_the_internship_starts()
    {
        var policy = new InternshipPolicy();
        var employee = new Employee { HireDate = new DateTime(2026, 11, 2) };

        var entitlement = policy.ComputeEntitlement(employee, new DateTime(2026, 10, 26));

        Assert.Equal(0.0, entitlement);
    }
}
