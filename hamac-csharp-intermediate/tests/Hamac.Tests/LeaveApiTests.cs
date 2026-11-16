using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hamac.Tests;

public class LeaveApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LeaveApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Balance_of_permanent_employee_is_annual_days_minus_days_taken()
    {
        var client = _factory.CreateClient();

        var balance = await client.GetFromJsonAsync<double>("/api/leave/balance?employeeId=2");

        Assert.Equal(13, balance, precision: 2);
    }

    [Fact]
    public async Task Approves_request_of_permanent_employee()
    {
        var client = _factory.CreateClient();

        var response = await client.GetStringAsync("/api/leave/approve?id=1");

        Assert.Equal("Approved", response);
    }

    [Fact]
    public async Task Balance_of_fixed_term_employee_counts_only_full_months()
    {
        var client = _factory.CreateClient();

        var balance = await client.GetFromJsonAsync<double>("/api/leave/balance?employeeId=4");

        Assert.Equal(4.56, balance, precision: 2);
    }
}
