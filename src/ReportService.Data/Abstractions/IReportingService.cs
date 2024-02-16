using ReportService.Data.Domain;

namespace ReportService.Data.Abstractions;

public interface IReportingService
{
    Task<Report> GetEmploeeDataByMonthAsync(DateTime date);
}
