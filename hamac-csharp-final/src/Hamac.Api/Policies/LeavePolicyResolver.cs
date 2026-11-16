namespace Hamac.Api.Policies;

public sealed class LeavePolicyResolver
{
    private readonly IReadOnlyDictionary<string, ILeaveEntitlementPolicy> _policies;

    public LeavePolicyResolver(IEnumerable<ILeaveEntitlementPolicy> policies) =>
        _policies = policies.ToDictionary(policy => policy.ContractType);

    public ILeaveEntitlementPolicy For(string contractType) =>
        _policies.TryGetValue(contractType, out var policy)
            ? policy
            : throw new InvalidOperationException($"Unknown contract type: {contractType}");
}
