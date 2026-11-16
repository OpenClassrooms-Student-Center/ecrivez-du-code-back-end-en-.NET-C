namespace Hamac.Api.Policies;

public sealed class LeavePolicyResolver
{
    private readonly IReadOnlyDictionary<string, ILeaveEntitlementPolicy> _policies;

    public LeavePolicyResolver(IReadOnlyDictionary<string, ILeaveEntitlementPolicy> policies) =>
        _policies = policies;

    public ILeaveEntitlementPolicy For(string contractType) =>
        _policies.TryGetValue(contractType, out var policy)
            ? policy
            : throw new InvalidOperationException($"Unknown contract type: {contractType}");
}
