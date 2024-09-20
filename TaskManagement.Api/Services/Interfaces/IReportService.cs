using System.Threading.Tasks;

namespace TaskManagement.Api.Services.Interfaces
{
    public interface IReportService
    {
        Task<object> GetPerformanceReport();
    }
}

