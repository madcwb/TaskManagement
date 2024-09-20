using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("performance")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> GetPerformanceReport()
        {
            var report = await _reportService.GetPerformanceReport();
            return Ok(report);
        }
    }
}

