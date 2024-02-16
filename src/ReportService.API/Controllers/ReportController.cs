using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.Data.Abstractions;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportingService _reportingService;

        public ReportController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            var date = new DateTime(year, month, 1);
            var result = await _reportingService.GetEmploeeDataByMonthAsync(date);
            
            var response = File(Encoding.ASCII.GetBytes(result.Data), "application/octet-stream", "report.txt");

            return Ok(response);
        }
    }
}
