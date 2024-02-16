using ReportService.Data.Domain;
using System.Net.Http.Json;

namespace ReportService.Data.Services;
public class SalaryApiClient
{
    private readonly HttpClient _httpClient;

    public SalaryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<int> GetSalaryAsync(Employee employee)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/empcode/{employee.Inn}", employee.BuhCode);
        response.EnsureSuccessStatusCode();
        var salary = await response.Content.ReadFromJsonAsync<int>();

        return salary;
    }
}
