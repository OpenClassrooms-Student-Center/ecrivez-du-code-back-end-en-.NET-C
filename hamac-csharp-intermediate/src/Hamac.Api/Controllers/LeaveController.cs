using Hamac.Api.Data;
using Hamac.Api.Policies;
using Hamac.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hamac.Api.Controllers;

[ApiController]
[Route("api/leave")]
public class LeaveController : ControllerBase
{
    private readonly LeaveService _leaveService;

    public LeaveController()
    {
        // Assemblage provisoire : il sera confié au conteneur .NET au chapitre 4.
        var policies = new LeavePolicyResolver(new Dictionary<string, ILeaveEntitlementPolicy>
        {
            [ContractTypes.Permanent] = new PermanentContractPolicy(),
            [ContractTypes.FixedTerm] = new FixedTermContractPolicy(),
            [ContractTypes.Apprenticeship] = new ApprenticeshipPolicy(),
        });

        _leaveService = new LeaveService(new HamacDbContext(), policies, new NotificationService());
    }

    [HttpGet("balance")]
    public IActionResult GetBalance(int employeeId)
    {
        var balance = _leaveService.GetBalance(employeeId);
        return balance is null ? Ok("Employee not found") : Ok(balance);
    }

    [HttpGet("approve")]
    public IActionResult Approve(int id) => _leaveService.Approve(id) switch
    {
        ApprovalResult.Approved => Ok("Approved"),
        ApprovalResult.InsufficientBalance => Ok("Refused: insufficient balance"),
        _ => Ok("Leave request not found"),
    };
}
