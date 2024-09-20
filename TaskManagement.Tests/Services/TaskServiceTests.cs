using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TaskServiceTests
{
    private readonly TaskService _taskService;
    private readonly AppDbContext _dbContext;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                      .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                      .Options;

        _dbContext = new AppDbContext(options);
        _taskService = new TaskService(_dbContext);
    }

    [Fact]
    public async Task CreateTask_ShouldThrowErrorWhenTaskLimitExceeded()
    {
        var project = new Project
        {
            Id = 5,
            Name = "Projeto 1",
            UserId = 1,
            Tasks = new List<TaskModel>(Enumerable.Repeat(new TaskModel
            {
                Title = "Nova tarefa",
                Description = "Nova descrição da tarefa",
                DueDate = DateTime.Now.AddDays(7),
                Priority = "Alta",
                Status = "Pendente"
            }, 20))
        };
        

        await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();

        var task = new TaskModel
        {
            Title = "Nova tarefa",
            Description = "Nova descrição da tarefa",
            DueDate = DateTime.Now.AddDays(7),
            Priority = "Alta",
            Status = "Pendente",
            ProjectId = project.Id
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.CreateTask(task, 5));
    }

    [Fact]
    public async Task DeleteTask_ShouldRemoveTaskAndRelatedHistory()
    {
        var project = new Project
        {
            Id = 30,
            Name = "Projeto 1",
            UserId = 1
        };

        var task = new TaskModel
        {
            Id = 1,
            Title = "Test Task",
            Description = "Teste descrição da tarefa",
            DueDate = DateTime.Now.AddDays(5),
            Status = "Concluída",
            Priority = "Média",
            ProjectId = project.Id
        };

        var history = new TaskHistory
        {
            Id = 1,
            TaskId = 1,
            Update = "Historico da tarefa 1",
            ModificationDate = DateTime.Now,
            User = "Test User"
        };

        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.TaskHistories.AddAsync(history);
        await _dbContext.SaveChangesAsync();

        await _taskService.DeleteTask(1);

        Assert.Null(await _dbContext.Tasks.FindAsync(1));
        Assert.Empty(_dbContext.TaskHistories.Where(h => h.TaskId == 1));
    }

    [Fact]
    public async Task AddComment_ShouldRecordCommentInHistory()
    {
        var project = new Project
        {
            Id = 1,
            Name = "Projeto 1",
            UserId = 1
        };

        var task = new TaskModel
        {
            Id = 1,
            Title = "Tarefa 1",
            Description = "Tarefa Description",
            Status = "Pendente",
            DueDate = DateTime.Now.AddDays(5),
            Priority = "Alta",
            ProjectId = project.Id
        };

        await _dbContext.Projects.AddAsync(project);
        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        // Usar o serviço para adicionar o comentário e registrar o histórico
        await _taskService.AddComment(1, "Comentario de teste");

        // Verifica se o comentário foi adicionado
        var addedComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.TaskId == 1);
        Assert.NotNull(addedComment);
        Assert.Equal("Comentario de teste", addedComment.Text);

        // Verifica se o histórico foi registrado corretamente
        var history = await _dbContext.TaskHistories.FirstOrDefaultAsync(h => h.TaskId == 1);
        Assert.NotNull(history);
        Assert.Equal("Comment added: Comentario de teste", history.Update);
    }


    [Fact]
    public async Task UpdateTask_ShouldUpdateTaskAndRecordHistory()
    {
        var project = new Project { Id = 1, Name = "Projeto 1", UserId = 1 };
        var task = new TaskModel
        {
            Id = 1,
            Title = "Tarefa 1",
            Description = "Descrição",
            Status = "Pendente",
            Priority = "Média",
            ProjectId = project.Id
        };

        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        // Atualiza a tarefa
        var updatedTask = new TaskModel
        {
            Id = 1,
            Title = "Tarefa atualizada 1",
            Description = "Descrição atualizada",
            Status = "Concluída",
            Priority = "Média",
            ProjectId = project.Id
        };

        await _taskService.UpdateTask(updatedTask);

        var history = _dbContext.TaskHistories.FirstOrDefault(h => h.TaskId == 1);
        Assert.NotNull(history);
        Assert.Equal("Tarefa atualizada", history.Update);
    }

    [Fact]
    public async Task UpdateTask_ShouldThrowError_WhenPriorityIsChanged()
    {
        var project = new Project { Id = 1, Name = "Projeto 1", UserId = 1 };
        var task = new TaskModel
        {
            Id = 1,
            Title = "Tarefa 1",
            Description = "Descrição",
            Status = "Pendente",
            Priority = "Média",
            ProjectId = project.Id
        };

        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        // Tenta atualizar a prioridade (isso deve falhar)
        var updatedTask = new TaskModel
        {
            Id = 1,
            Title = "Tarefa 1",
            Description = "Descrição",
            Status = "Pendente",
            Priority = "Alta",
            ProjectId = project.Id
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.UpdateTask(updatedTask));
    }



}
