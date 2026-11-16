using Hamac.Api.Data;
using Hamac.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hamac.Api.Controllers;

[ApiController]
[Route("api/leave")]
public class LeaveController : ControllerBase
{
    [HttpGet("balance")]
    public IActionResult GetBalance(int employeeId)
    {
        var db = new HamacDbContext();
        var e = db.Employees.Find(employeeId);
        if (e == null) return Ok("Employee not found");
        double days;
        if (e.ContractType == "CDI")
            days = 25;
        else if (e.ContractType == "CDD")
        {
            var months = Math.Ceiling((DateTime.Today - e.HireDate).TotalDays / 30);
            days = months * 2.08;
        }
        else if (e.ContractType == "Alternance")
            days = 25 + 5;
        else
            days = 0;
        return Ok(days - e.DaysTaken);
    }

    [HttpGet("approve")]
    public IActionResult Approve(int id)
    {
        var db = new HamacDbContext();
        var req = db.LeaveRequests.ToList().Where(r => r.Id == id).FirstOrDefault();
        var e = db.Employees.Find(req.EmployeeId);
        double d;
        if (e.ContractType == "CDI")
            d = 25;
        else if (e.ContractType == "CDD")
        {
            var m = Math.Floor((DateTime.Today - e.HireDate).TotalDays / 30);
            d = m * 2.08;
        }
        else if (e.ContractType == "Alternance")
            d = 30;
        else
            d = 0;
        if (d - e.DaysTaken < req.WorkingDays)
        {
            req.Status = "Refused";
            db.SaveChanges();
            return Ok("Refused: insufficient balance");
        }
        req.Status = "Approved";
        e.DaysTaken += req.WorkingDays;
        db.SaveChanges();
        new NotificationService().Notify(e, "Votre demande de congés est approuvée.");
        return Ok("Approved");
    }
}
