
using TaskManagement.Api.Models;
using System.Threading.Tasks;

namespace TaskManagement.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskModel> CreateTask(TaskModel task, int projectId);
        Task UpdateTask(TaskModel taskToUpdate);
        Task DeleteTask(int taskId);
        Task AddComment(int taskId, string commentText);
    }
}
