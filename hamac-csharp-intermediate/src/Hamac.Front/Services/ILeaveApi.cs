using Hamac.Front.Models;

namespace Hamac.Front.Services;

public interface ILeaveApi
{
    Task<double> GetBalanceAsync(int employeeId);
    Task<IReadOnlyList<LeaveRequestViewModel>> GetRequestsAsync(string? status);
    Task<LeaveRequestViewModel> CreateRequestAsync(NewLeaveRequestViewModel request);
    Task<LeaveRequestViewModel> ApproveAsync(int requestId);
    Task<LeaveRequestViewModel> RefuseAsync(int requestId);
}
