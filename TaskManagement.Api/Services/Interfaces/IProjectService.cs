
using TaskManagement.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagement.Api.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetProjectsByUser(int userId);
        Task<Project> CreateProject(Project project, int userId);
        Task<Project> GetProjectWithTasks(int projectId);
        Task<bool> CheckIfProjectHasPendingTasks(int projectId);
        
        Task DeleteProject(int projectId);
    }
}
