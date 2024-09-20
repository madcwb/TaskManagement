
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Services;
using System.Threading.Tasks;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services.Interfaces;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var userId = GetUserId();
            var projects = await _projectService.GetProjectsByUser(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            var userId = GetUserId();
            var newProject = await _projectService.CreateProject(project, userId);
            return CreatedAtAction(nameof(CreateProject), new { id = newProject.Id }, newProject);
        }

        private int GetUserId()
        {
            // Simulate logged-in user extraction
            return 1;
        }
    }
}
