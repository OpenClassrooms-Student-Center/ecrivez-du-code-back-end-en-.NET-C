using Hamac.Api.Policies;
using Hamac.Api.Models;

namespace Hamac.Api.Data;

public static class DemoData
{
    public static void Reset(HamacDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var today = DateTime.Today;

        // Salariés n° 1 à 5, dans cet ordre.
        db.Employees.Add(new Employee
        {
            FirstName = "Amina",
            LastName = "Diallo",
            ContractType = "CDI",
            HireDate = today.AddYears(-4).AddMonths(-2),
            DaysTaken = 8
        });
        db.Employees.Add(new Employee
        {
            FirstName = "Karim",
            LastName = "Benali",
            ContractType = "CDI",
            HireDate = today.AddYears(-6).AddMonths(-5),
            DaysTaken = 12
        });
        db.Employees.Add(new Employee
        {
            FirstName = "Mei",
            LastName = "Chen",
            ContractType = "Alternance",
            HireDate = today.AddYears(-1).AddMonths(-1),
            DaysTaken = 6
        });
        db.Employees.Add(new Employee
        {
            FirstName = "Leïla",
            LastName = "Haddad",
            ContractType = "CDD",
            HireDate = today.AddMonths(-7).AddDays(-5),
            DaysTaken = 10
        });
        db.Employees.Add(new Employee
        {
            FirstName = "Sofia",
            LastName = "Rossi",
            ContractType = "CDD",
            HireDate = today.AddMonths(-11).AddDays(-2),
            DaysTaken = 4
        });
        db.SaveChanges();

        // Salariée n° 6 : stage, qui commence dans 7 jours.
        db.Employees.Add(new Employee
        {
            FirstName = "Zoé",
            LastName = "Mendes",
            ContractType = ContractTypes.Internship,
            HireDate = DateTime.Today.AddDays(7),
            DaysTaken = 0
        });
        db.SaveChanges();

        // Demandes n° 1 à 8, dans cet ordre.
        db.LeaveRequests.AddRange(
            Request(1, today.AddDays(14), today.AddDays(18), 5, "Pending"),
            Request(3, today.AddDays(-40), today.AddDays(-38), 3, "Approved"),
            Request(2, today.AddDays(30), today.AddDays(31), 2, "Pending"),
            Request(5, today.AddDays(-20), today.AddDays(-17), 4, "Approved"),
            Request(3, today.AddDays(21), today.AddDays(25), 5, "Refused"),
            Request(2, today.AddDays(-60), today.AddDays(-56), 5, "Approved"),
            Request(4, today.AddDays(35), today.AddDays(39), 5, "Pending"),
            Request(5, today.AddDays(45), today.AddDays(46), 2, "Pending"));
        db.SaveChanges();
    }

    private static LeaveRequest Request(int employeeId, DateTime start, DateTime end, int workingDays, string status) =>
        new()
        {
            EmployeeId = employeeId,
            StartDate = start,
            EndDate = end,
            WorkingDays = workingDays,
            Status = status
        };
}
