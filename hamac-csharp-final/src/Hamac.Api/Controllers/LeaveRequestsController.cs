using Hamac.Api.Dtos;
using Hamac.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hamac.Api.Controllers;

[ApiController]
[Route("api/leave-requests")]
public sealed class LeaveRequestsController : ControllerBase
{
    private readonly LeaveService _leaveService;

    public LeaveRequestsController(LeaveService leaveService) =>
        _leaveService = leaveService;

    [HttpGet]
    public IEnumerable<LeaveRequestDto> GetAll(string? status) =>
        _leaveService.GetRequests(status).Select(LeaveRequestDto.From);

    [HttpGet("{id:int}")]
    public ActionResult<LeaveRequestDto> GetById(int id)
    {
        var request = _leaveService.GetRequest(id);
        if (request is null) return NotFound();

        return LeaveRequestDto.From(request);
    }

    [HttpPost]
    public ActionResult<LeaveRequestDto> Create(CreateLeaveRequestDto dto)
    {
        var request = _leaveService.Create(dto.EmployeeId, dto.StartDate!.Value, dto.EndDate!.Value);
        if (request is null)
        {
            ModelState.AddModelError(nameof(dto.EmployeeId), "Ce salarié n’existe pas.");
            return ValidationProblem();
        }

        return CreatedAtAction(nameof(GetById), new { id = request.Id }, LeaveRequestDto.From(request));
    }

    [HttpPost("{id:int}/approval")]
    public ActionResult<LeaveRequestDto> Approve(int id) => _leaveService.Approve(id) switch
    {
        ApprovalResult.NotFound => NotFound(),
        ApprovalResult.NotPending => AlreadyProcessed(),
        _ => LeaveRequestDto.From(_leaveService.GetRequest(id)!),
    };

    [HttpPost("{id:int}/refusal")]
    public ActionResult<LeaveRequestDto> Refuse(int id) => _leaveService.Refuse(id) switch
    {
        RefusalResult.NotFound => NotFound(),
        RefusalResult.NotPending => AlreadyProcessed(),
        _ => LeaveRequestDto.From(_leaveService.GetRequest(id)!),
    };

    private ObjectResult AlreadyProcessed() =>
        Problem(statusCode: StatusCodes.Status409Conflict, title: "Cette demande a déjà été traitée.");
}
