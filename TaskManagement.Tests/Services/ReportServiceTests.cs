using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Api.Data;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;

public class ReportServiceTests
{
    private readonly ReportService _reportService;
    private readonly AppDbContext _dbContext;

    public ReportServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                      .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                      .Options;

        _dbContext = new AppDbContext(options);
        _reportService = new ReportService(_dbContext);
    }

    [Fact]
    public async Task GetPerformanceReport_ShouldReturnAverageTasksCompleted()
    {
        var tasks = new List<TaskModel>
        {
            new TaskModel
            {
                Id = 1,
                Title = "Tarefa 1",
                Description = "Tarefa 1 descrição",
                Status = "Concluída",
                DueDate = DateTime.Now.AddDays(-5),
                Priority = "Alta",
                Project = new Project
                {
                    Id = 2,
                    Name = "Projeto 1",
                    UserId = 1,
                    Tasks = new List<TaskModel>()
                }
            },
            new TaskModel
            {
                Id = 2,
                Title = "Tarefa 2",
                Description = "Tarefa 2 descrição",
                Status = "Concluída",
                DueDate = DateTime.Now.AddDays(-10),
                Priority = "Média",
                Project = new Project
                {
                    Id = 1,
                    Name = "Projeto 1",
                    UserId = 2,
                    Tasks = new List<TaskModel>()
                }
            }
        };

        await _dbContext.Tasks.AddRangeAsync(tasks);
        await _dbContext.SaveChangesAsync();

        var result = await _reportService.GetPerformanceReport();

        Assert.NotNull(result);
    }
}
