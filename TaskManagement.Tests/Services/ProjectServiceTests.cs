using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;

public class ProjectServiceTests
{
    private readonly ProjectService _projectService;
    private readonly AppDbContext _dbContext;

    public ProjectServiceTests()
    {
        // Usar o InMemoryDatabase para evitar problemas com mocks
        var options = new DbContextOptionsBuilder<AppDbContext>()
                      .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                      .Options;

        _dbContext = new AppDbContext(options);
        _projectService = new ProjectService(_dbContext);
    }

    [Fact]
    public async Task GetProjectsByUser_ShouldReturnProjectsForUser()
    {
        var projects = new List<Project>
        {
            new Project { Id = 10, Name = "Projeto 1", UserId = 1 },
            new Project { Id = 20, Name = "Projeto 2", UserId = 1 }
        };

        // Adiciona os projetos no banco de dados em memória
        await _dbContext.Projects.AddRangeAsync(projects);
        await _dbContext.SaveChangesAsync();

        var result = await _projectService.GetProjectsByUser(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeleteProject_ShouldThrowError_WhenProjectHasPendingTasks()
    {
        var project = new Project
        {
            Id = 1,
            Name = "Projeto 1",
            UserId = 1,
            Tasks = new List<TaskModel>()
        };

        var task = new TaskModel
        {
            Id = 1,
            Title = "Tarefa pendente",
            Status = "Pendente",
            Description = "Descrição",
            DueDate = DateTime.Now.AddDays(5),
            Priority = "Média",
            ProjectId = 1
        };

        project.Tasks.Add(task); // Associa a tarefa ao projeto

        // Adicionando dados via contexto para simular o banco de dados em memória
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        // Acessar o serviço e tentar remover o projeto
        await Assert.ThrowsAsync<InvalidOperationException>(() => _projectService.DeleteProject(1));
    }


    [Fact]
    public async Task DeleteProject_ShouldRemoveProject_WhenNoPendingTasks()
    {
        var project = new Project
        {
            Id = 1,
            Name = "Projeto 1",
            UserId = 1,
            Tasks = new List<TaskModel>()
        };

        var task = new TaskModel
        {
            Id = 1,
            Title = "Tarefa concluida",
            Status = "Concluída",
            Description = "Descrição",
            DueDate = DateTime.Now.AddDays(-5),
            Priority = "Alta",
            ProjectId = project.Id
        };

        project.Tasks.Add(task); // Associa a tarefa ao projeto

        // Adicionando dados via contexto para simular o banco de dados em memória
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        // Acessar o serviço e remover o projeto
        await _projectService.DeleteProject(1);

        // Verificar se o projeto foi removido corretamente
        Assert.Null(await _dbContext.Projects.FindAsync(1));
    }





}
