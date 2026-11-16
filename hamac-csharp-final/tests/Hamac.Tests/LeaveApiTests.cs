using System.Net;
using System.Net.Http.Json;
using Hamac.Api.Dtos;
using Hamac.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

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

        var balance = await client.GetFromJsonAsync<BalanceDto>("/api/employees/2/balance");

        Assert.Equal(13, balance!.Balance, precision: 2);
    }

    [Fact]
    public async Task Approves_request_of_permanent_employee()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/leave-requests/1/approval", null);
        var request = await response.Content.ReadFromJsonAsync<LeaveRequestDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(LeaveStatuses.Approved, request!.Status);
    }

    [Fact]
    public async Task Balance_of_fixed_term_employee_counts_only_full_months()
    {
        var client = _factory.CreateClient();

        var balance = await client.GetFromJsonAsync<BalanceDto>("/api/employees/4/balance");

        Assert.Equal(4.56, balance!.Balance, precision: 2);
    }

    [Fact]
    public async Task Approving_a_request_twice_returns_conflict()
    {
        var client = _factory.CreateClient();

        await client.PostAsync("/api/leave-requests/7/approval", null);
        var response = await client.PostAsync("/api/leave-requests/7/approval", null);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Creating_a_request_ending_before_it_starts_returns_bad_request()
    {
        var client = _factory.CreateClient();
        var dto = new { employeeId = 4, startDate = "2026-07-10", endDate = "2026-07-06" };

        var response = await client.PostAsJsonAsync("/api/leave-requests", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Balance_of_intern_is_available_before_arrival()
    {
        var client = _factory.CreateClient();

        var balance = await client.GetFromJsonAsync<BalanceDto>("/api/employees/6/balance");

        Assert.Equal(0.0, balance!.Balance);
    }

    [Fact]
    public void Container_can_build_leave_service()
    {
        using var scope = _factory.Services.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<LeaveService>();

        Assert.NotNull(service);
    }
}
