namespace Hamac.Api.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string ContractType { get; set; } = "";
    public DateTime HireDate { get; set; }
    public double DaysTaken { get; set; }
}
