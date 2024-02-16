using ReportService.Data.Domain;

namespace ReportService.Data.Services;

public class BuhApiClient
{
    private readonly HttpClient _httpClient;

    public BuhApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<string> GetCodeAsync(Employee employee)
    {
        return await _httpClient.GetStringAsync("api/inn/" + employee.Inn);
    }
}
