using Hamac.Front.Models;

namespace Hamac.Front.Services;

// Données simulées, écrites en dur en attendant le branchement de l'API.
public class MockLeaveApi : ILeaveApi
{
    private readonly List<LeaveRequestViewModel> _requests = new()
    {
        new() { Id = 1, EmployeeId = 1, StartDate = new DateTime(2026, 7, 6), EndDate = new DateTime(2026, 7, 10), Days = 5, Status = "Pending" },
        new() { Id = 2, EmployeeId = 2, StartDate = new DateTime(2026, 5, 18), EndDate = new DateTime(2026, 5, 20), Days = 3, Status = "Approved" },
        new() { Id = 3, EmployeeId = 3, StartDate = new DateTime(2026, 8, 3), EndDate = new DateTime(2026, 8, 7), Days = 5, Status = "Refused" },
        new() { Id = 4, EmployeeId = 1, StartDate = new DateTime(2026, 9, 14), EndDate = new DateTime(2026, 9, 15), Days = 2, Status = "Pending" }
    };

    public Task<double> GetBalanceAsync(int employeeId)
    {
        if (employeeId <= 0)
            throw new LeaveApiException("Salarié introuvable.");
        return Task.FromResult(12.5);
    }

    public Task<IReadOnlyList<LeaveRequestViewModel>> GetRequestsAsync(string? status)
    {
        IReadOnlyList<LeaveRequestViewModel> result = _requests
            .Where(r => status == null || r.Status == status)
            .OrderBy(r => r.StartDate)
            .ToList();
        return Task.FromResult(result);
    }

    public Task<LeaveRequestViewModel> CreateRequestAsync(NewLeaveRequestViewModel request)
    {
        if (request.StartDate is null || request.EndDate is null)
            throw new LeaveApiException("Les dates de début et de fin sont obligatoires.");
        if (request.EndDate < request.StartDate)
            throw new LeaveApiException("La date de fin doit suivre la date de début.");

        var created = new LeaveRequestViewModel
        {
            Id = _requests.Max(r => r.Id) + 1,
            EmployeeId = request.EmployeeId,
            StartDate = request.StartDate.Value,
            EndDate = request.EndDate.Value,
            Days = (request.EndDate.Value - request.StartDate.Value).Days + 1,
            Status = "Pending"
        };
        _requests.Add(created);
        return Task.FromResult(created);
    }

    public Task<LeaveRequestViewModel> ApproveAsync(int requestId) => Decide(requestId, "Approved");

    public Task<LeaveRequestViewModel> RefuseAsync(int requestId) => Decide(requestId, "Refused");

    private Task<LeaveRequestViewModel> Decide(int requestId, string status)
    {
        var request = _requests.FirstOrDefault(r => r.Id == requestId)
            ?? throw new LeaveApiException("Demande introuvable.");
        if (request.Status != "Pending")
            throw new LeaveApiException("Cette demande a déjà été traitée.");
        request.Status = status;
        return Task.FromResult(request);
    }
}
