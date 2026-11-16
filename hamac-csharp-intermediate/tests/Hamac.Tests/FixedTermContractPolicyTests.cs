using Hamac.Api.Models;
using Hamac.Api.Policies;

namespace Hamac.Tests;

public class FixedTermContractPolicyTests
{
    [Fact]
    public void Fixed_term_policy_counts_only_full_months()
    {
        var policy = new FixedTermContractPolicy();
        var employee = new Employee { HireDate = new DateTime(2026, 3, 15) };

        var entitlement = policy.ComputeEntitlement(employee, new DateTime(2026, 10, 20));

        Assert.Equal(14.56, entitlement, precision: 2);
    }
}
