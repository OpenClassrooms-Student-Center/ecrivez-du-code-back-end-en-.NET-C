using Hamac.Api.Models;

namespace Hamac.Api.Services;

public interface ILeaveNotifier
{
    void Notify(Employee employee, string message);
}
