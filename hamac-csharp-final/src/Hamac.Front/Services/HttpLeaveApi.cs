using System.Net.Http.Json;
using Hamac.Front.Models;

namespace Hamac.Front.Services;

public sealed class HttpLeaveApi : ILeaveApi
{
    private readonly HttpClient _http;

    public HttpLeaveApi(HttpClient http) => _http = http;

    public async Task<double> GetBalanceAsync(int employeeId)
    {
        var response = await _http.GetAsync($"api/employees/{employeeId}/balance");
        var balance = await ReadAsync<BalanceResponse>(response);
        return balance.Balance;
    }

    public async Task<IReadOnlyList<LeaveRequestViewModel>> GetRequestsAsync(string? status)
    {
        var url = status is null
            ? "api/leave-requests"
            : $"api/leave-requests?status={Uri.EscapeDataString(status)}";

        return await ReadAsync<List<LeaveRequestViewModel>>(await _http.GetAsync(url));
    }

    public async Task<LeaveRequestViewModel> CreateRequestAsync(NewLeaveRequestViewModel request) =>
        await ReadAsync<LeaveRequestViewModel>(await _http.PostAsJsonAsync("api/leave-requests", request));

    public async Task<LeaveRequestViewModel> ApproveAsync(int requestId) =>
        await ReadAsync<LeaveRequestViewModel>(await _http.PostAsync($"api/leave-requests/{requestId}/approval", null));

    public async Task<LeaveRequestViewModel> RefuseAsync(int requestId) =>
        await ReadAsync<LeaveRequestViewModel>(await _http.PostAsync($"api/leave-requests/{requestId}/refusal", null));

    private static async Task<T> ReadAsync<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return (await response.Content.ReadFromJsonAsync<T>())!;
        }

        throw new LeaveApiException(await ReadErrorMessageAsync(response));
    }

    private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        var fallback = $"Erreur {(int)response.StatusCode}";

        if (response.Content.Headers.ContentType?.MediaType != "application/problem+json")
        {
            return fallback;
        }

        var problem = await response.Content.ReadFromJsonAsync<ApiProblem>();

        return problem?.Errors?.Values.SelectMany(messages => messages).FirstOrDefault()
            ?? problem?.Title
            ?? fallback;
    }

    private sealed record BalanceResponse(double Balance);

    private sealed record ApiProblem(string? Title, Dictionary<string, string[]>? Errors);
}
