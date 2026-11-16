using Hamac.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hamac.Api.Data;

public sealed class HamacDbContext : DbContext
{
    public HamacDbContext(DbContextOptions<HamacDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
}
