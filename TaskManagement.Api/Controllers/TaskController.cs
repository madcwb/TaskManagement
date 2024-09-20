
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Services.Interfaces;
using System.Threading.Tasks;
using TaskManagement.Api.Models;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(int projectId, [FromBody] TaskModel task)
        {
            try
            {
                var newTask = await _taskService.CreateTask(task, projectId);
                return CreatedAtAction(nameof(CreateTask), new { id = newTask.Id }, newTask);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskModel task)
        {
            try
            {
                task.Id = taskId;
                await _taskService.UpdateTask(task);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Remover Tarefa
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                await _taskService.DeleteTask(taskId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
