using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Api.Data;
using TaskManagement.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetPerformanceReport()
        {
            var tasksCompleted = await _context.Tasks
                .Where(t => t.Status == "Concluída" && t.DueDate >= DateTime.Now.AddDays(-30))
                .GroupBy(t => t.Project.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    AverageTasksCompleted = g.Count() / 30
                })
                .ToListAsync();

            return tasksCompleted;
        }
    }
}
