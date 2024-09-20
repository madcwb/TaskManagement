using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Api.Data;
using TaskManagement.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Models;

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
            //var tasksCompleted = await _context.Tasks                
            //    .Where(t => t.Status == "Concluída" && t.DueDate >= DateTime.Now.AddDays(-30))
            //    .GroupBy(t => t.ProjectId)
            //    .Select(g => new
            //    {
            //        UserId = g.Key,
            //        AverageTasksCompleted = g.Count() / 30
            //    })
            //    .ToListAsync();


            var tasksCompleted = from t in _context.Tasks
                                  join p in _context.Projects on t.ProjectId equals p.Id
                                  group p by p.UserId into g
                                  select new
                                  {
                                      UserId = g.Key,
                                      AverageTasksCompleted = g.Count() / 30
                                  };


            return tasksCompleted;
        }
    }
}
