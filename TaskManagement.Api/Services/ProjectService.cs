
using TaskManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Api.Services.Interfaces;
using TaskManagement.Api.Data;

namespace TaskManagement.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetProjectsByUser(int userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<Project> CreateProject(Project project, int userId)
        {
            project.UserId = userId;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> GetProjectWithTasks(int projectId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<bool> CheckIfProjectHasPendingTasks(int projectId)
        {
            return await _context.Tasks.AnyAsync(t => t.ProjectId == projectId && t.Status == "Pending");
        }

        public async Task DeleteProject(int projectId)
        {
            // Obter o projeto com suas tarefas associadas
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new KeyNotFoundException("Projeto not found");
            }

            // Verificar se há tarefas pendentes
            if (project.Tasks.Any(t => t.Status == "Pendente"))
            {
                throw new InvalidOperationException("Cannot delete project with pending tasks.");
            }

            // Se não houver tarefas pendentes, remover o projeto
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

    }
}
