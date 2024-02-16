using ReportService.Data.Repositories;
using ReportService.Data.Abstractions;
using ReportService.Data.Domain;

namespace ReportService.Data.Services;
public class ReportingService : IReportingService
{
    private readonly IReportingRepository _reportingRepository;
    private readonly BuhApiClient _buhApiClient;
    private readonly SalaryApiClient _salaryApiClient;

    public ReportingService(IReportingRepository reportingRepository, BuhApiClient buhApiClient, SalaryApiClient salaryApiClient)
    {
        _reportingRepository = reportingRepository;
        _buhApiClient = buhApiClient;
        _salaryApiClient = salaryApiClient;
    }

    public async Task<Report> GetEmploeeDataByMonthAsync(DateTime dateTime)
    {
        var report = new Report();

        var emploee = await _reportingRepository.GetEmployeesAsync();

        if (emploee != null && emploee.Any())
        {
            var formatter = new ReportFormatter();

            formatter.AddPeriod(dateTime);
            formatter.AddNewLine();
            formatter.AddNewLine();

            foreach (var group in emploee.GroupBy(e => e.Department))
            {
                formatter.AddWordLine();
                formatter.AddNewLine();
                formatter.AddWord(group.Key);
                formatter.AddNewLine();

                foreach (var emp in group.ToArray())
                {
                    emp.BuhCode = await _buhApiClient.GetCodeAsync(emp);
                    emp.Salary = await _salaryApiClient.GetSalaryAsync(emp);
                    formatter.AddNewLine();
                    formatter.AddEmployeeName(emp);
                    formatter.AddWordTab();
                    formatter.AddSalary(emp);
                    formatter.AddNewLine();
                }

                formatter.AddNewLine();
                formatter.AddWord("Всего по отделу");
                formatter.AddWordTab();
                formatter.AddSalarySum(group.Sum(g => g.Salary));
                formatter.AddNewLine();
                formatter.AddNewLine();
            }

            formatter.AddWordLine();
            formatter.AddNewLine();
            formatter.AddNewLine();
            formatter.AddWord("Всего по предприятию");
            formatter.AddWordTab();
            formatter.AddSalarySum(emploee.Sum(g => g.Salary));

            report.Data = formatter.Build();
        }

        return report;
    }
}
