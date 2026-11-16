using Hamac.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hamac.Api.Data;

public class HamacDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Data Source=hamac.db");
}
