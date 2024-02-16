using System;
using System.Text;
using Npgsql;
using ReportingService.Data.Domain;
using ReportingService.Data.Repositories;
using ReportService.Data.Abstractions;
using ReportService.Data.Domain;
using ReportService.Domain;

namespace ReportService.Data.Services;
public class ReportingService : IReportingService
{
    private readonly IReportingRepository _reportingRepository;

    public ReportingService(IReportingRepository reportingRepository)
    {
        _reportingRepository = reportingRepository;
    }

    public async Task<Report> GetEmploeeDataByMonthAsync(DateTime dateTime)
    {
        var report = new Report();

        List<Employee> emplist = new List<Employee>();
        var emploee = await _reportingRepository.GetEmployeesAsync();
        var formatter = new ReportFormatter();

        foreach (var group in emploee.GroupBy(e => e.Department))
        {
            
            formatter.AddNewLine();
            formatter.AddWordLine();
            formatter.AddNewLine();
            formatter.AddWord(group.Key);
            formatter.AddNewLine();

            foreach (var emp in group.ToArray())
            {
                emp.BuhCode = await EmpCodeResolver.GetCode(emp.Inn);
                emp.Salary = emp.Salary();
                formatter.AddEmployeeName(emp);
                formatter.AddWordTab();
                formatter.AddSalary(emp);
                formatter.AddNewLine();
            }

            formatter.AddNewLine();
            formatter.AddWord("Всего по отделу");
            formatter.AddWordTab();
            formatter.AddSalarySum(group.Sum(g => g.Salary));
        }

        formatter.AddNewLine();
        formatter.AddWord("Всего по предприятию");
        formatter.AddWordTab();
        formatter.AddSalarySum(emploee.Sum(g => g.Salary));

        report.Data = sb.ToString();

        return report;
    }
}
