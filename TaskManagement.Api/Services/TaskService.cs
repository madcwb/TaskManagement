
using TaskManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskManagement.Api.Services.Interfaces;
using TaskManagement.Api.Data;

namespace TaskManagement.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskModel> CreateTask(TaskModel task, int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);

            if (project.Tasks?.Count >= 20)
                throw new InvalidOperationException("Tarefa limit for this project is 20.");

            task.ProjectId = projectId;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateTask(TaskModel taskToUpdate)
        {
            var task = await _context.Tasks.FindAsync(taskToUpdate.Id);
            if (task == null) throw new KeyNotFoundException("Tarefa not found");

            // Impedir alteração de prioridade após a criação
            if (task.Priority != taskToUpdate.Priority)
                throw new InvalidOperationException("Tarefa priority cannot be changed after creation.");

            task.Update(taskToUpdate);
            _context.Entry(task).State = EntityState.Modified;

            AddTaskHistory(taskToUpdate.Id, "Tarefa atualizada");

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Tarefa not found");

            // Remover a tarefa e também qualquer registro de histórico relacionado a ela
            var taskHistories = _context.TaskHistories.Where(h => h.TaskId == taskId);
            _context.TaskHistories.RemoveRange(taskHistories);

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        private void AddTaskHistory(int taskId, string update)
        {
            var history = new TaskHistory
            {
                TaskId = taskId,
                Update = update,
                ModificationDate = DateTime.Now,
                User = "Logged-in User" // Simulating user
            };
            _context.TaskHistories.Add(history);
        }

        public async Task AddComment(int taskId, string commentText)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException("Tarefa not found");

            var comment = new Comment
            {
                TaskId = taskId,
                Text = commentText,
                CommentDate = DateTime.Now
            };

            // Adiciona o comentário
            await _context.Comments.AddAsync(comment);

            // Registra a adição de comentário no histórico da tarefa
            var history = new TaskHistory
            {
                TaskId = taskId,
                Update = $"Comment added: {commentText}",
                ModificationDate = DateTime.Now,
                User = "Test User"  // Aqui estamos simulando o usuário
            };

            await _context.TaskHistories.AddAsync(history);

            await _context.SaveChangesAsync();
        }

    }
}
