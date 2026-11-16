using Hamac.Api.Dtos;
using Hamac.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hamac.Api.Controllers;

[ApiController]
[Route("api/employees")]
public sealed class EmployeesController : ControllerBase
{
    private readonly LeaveService _leaveService;

    public EmployeesController(LeaveService leaveService) =>
        _leaveService = leaveService;

    [HttpGet("{id:int}/balance")]
    public ActionResult<BalanceDto> GetBalance(int id)
    {
        var balance = _leaveService.GetBalance(id);
        if (balance is null) return NotFound();

        return new BalanceDto(id, balance.Value);
    }
}
