using Npgsql;
using System.Data;
using ReportingService.Data.Domain;
using Dapper;

namespace ReportingService.Data.Repositories;

public interface IReportingRepository
{
    Task<IReadOnlyCollection<Employee>> GetEmployeesAsync();
}

public class ReportingRepository : IReportingRepository
{
    private readonly NpgsqlConnection _dbConnection;

    public ReportingRepository(IDbConnection dbConnection)
    {
        _dbConnection = (NpgsqlConnection)dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    public async Task<IReadOnlyCollection<Employee>> GetEmployeesAsync()
    {
        if (_dbConnection.State != ConnectionState.Open) await _dbConnection.OpenAsync();
        using var command = new NpgsqlCommand();
        command.Connection = _dbConnection;

        var sql = $@"
            SELECT 
                e.name AS {nameof(Employee.Name)}, 
                e.inn AS {nameof(Employee.Inn)}, 
                d.name AS {nameof(Employee.Department)} 
            FROM emps e 
            LEFT JOIN deps d on e.departmentid = d.id AND d.active = true";

        var result = (await _dbConnection.QueryAsync<Employee>(sql)).ToArray();

        return result;
    }
}
