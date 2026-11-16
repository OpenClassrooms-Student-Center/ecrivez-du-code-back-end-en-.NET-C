using Hamac.Api.Models;

namespace Hamac.Api.Services;

public class NotificationService : ILeaveNotifier
{
    private readonly List<(Employee Employee, string Message)> _failed = new();

    public void Notify(Employee employee, string message)
    {
        try
        {
            Send($"{employee.FirstName} {employee.LastName}", message);
        }
        catch (Exception)
        {
            _failed.Add((employee, message));
        }
    }

    public void SendWeeklySummary(Employee manager, IEnumerable<LeaveRequest> pendingRequests)
    {
        var count = pendingRequests.Count();
        Send($"{manager.FirstName} {manager.LastName}",
            $"Récapitulatif hebdomadaire : {count} demande(s) en attente.");
    }

    public int RetryFailedNotifications()
    {
        var toRetry = _failed.ToList();
        _failed.Clear();
        foreach (var (employee, message) in toRetry)
        {
            Notify(employee, message);
        }
        return toRetry.Count - _failed.Count;
    }

    private static void Send(string recipient, string message)
    {
        // Simulation d'envoi : dans la vraie application, un service de messagerie.
        Console.WriteLine($"[Notification] {recipient} : {message}");
    }
}
