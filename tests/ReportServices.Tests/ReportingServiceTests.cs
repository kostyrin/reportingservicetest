using Moq;
using ReportService.Data.Domain;
using ReportService.Data.Repositories;
using ReportService.Data.Services;
using Shouldly;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReportingServices.Tests;

public class Tests
{
    private ReportingService _reportingService;
    private Mock<IReportingRepository> _repositoryMock;
    private Mock<BuhApiClient> _buhApiClientMock;
    private Mock<SalaryApiClient> _salaryApiClientMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IReportingRepository>();
        _buhApiClientMock = new Mock<BuhApiClient>(Mock.Of<HttpClient>());
        _salaryApiClientMock = new Mock<SalaryApiClient>(Mock.Of<HttpClient>());
        _reportingService = new ReportingService(_repositoryMock.Object, _buhApiClientMock.Object, _salaryApiClientMock.Object);

    }

    [Test]
    public async Task GetEmploeeDataByMonthAsync_CurrentDate_ShouldReportOk()
    {
        //Arrange
        var date = DateTime.Now;
        var list = new List<Employee>
        {
            new Employee
            {
                Name = "Андрей Сергеевич Бубнов",
                Department = "ФинОтдел",
                Inn = "9999999"
            },
            new Employee
            {
                Name = "Григорий Евсеевич Зиновьев",
                Department = "ФинОтдел",
                Inn = "99998888"
            },
            new Employee
            {
                Name = "Андрей Павлович Кириленко",
                Department = "ИТ",
                Inn = "11111111"
            }
        };
        _repositoryMock.Setup(x => x.GetEmployeesAsync()).ReturnsAsync(list);
        _buhApiClientMock.Setup(x => x.GetCodeAsync(It.IsAny<Employee>())).ReturnsAsync("123");
        _salaryApiClientMock.Setup(x => x.GetSalaryAsync(It.IsAny<Employee>())).ReturnsAsync(10000);

        //Act
        var result = await _reportingService.GetEmploeeDataByMonthAsync(date);

        //Assert
        result.ShouldNotBeNull();
        result.Data.ShouldNotBeNull();
        result.Data.ShouldContain(date.ToString("MMMM yyyy", CultureInfo.CurrentCulture));
    }
}