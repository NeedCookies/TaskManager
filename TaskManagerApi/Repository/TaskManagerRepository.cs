using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Abstractions;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Repository
{
    public class TaskManagerRepository : ITaskManagerRepository
    {
        private readonly TaskManagerDbContext _dbContext;
        private readonly ILogger<TaskManagerRepository> _logger;

        public TaskManagerRepository(TaskManagerDbContext dbContext,
            ILogger<TaskManagerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<TaskItem>> Get()
        {
            return await _dbContext.Tasks
                .AsNoTracking()
                .OrderBy(task => task.Name)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetById(int id)
        {
            var taskItem = await _dbContext.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(task => task.Id == id);

            if (taskItem == null)
            {
                _logger.LogInformation("User looks for task which doesn't exist, return null");
            }

            return taskItem;
        }

        public async Task CreateTask(string name, string description, string type, bool isCompleted, DateTime deadLine)
        {
            var taskItem = new TaskItem();
            taskItem.Name = name;
            taskItem.Description = description;
            taskItem.Type = type;
            taskItem.IsCompleted = isCompleted;
            taskItem.DeadLine = deadLine;
            await _dbContext.Tasks.AddAsync(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTaskById(int id, string name, string description, string type, bool isCompleted, DateTime deadLine)
        {
            var taskItem = await _dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id);
            if (taskItem == null)
            {
                _logger.LogInformation("User tried to update non-existent task");
                throw new NullReferenceException();
            }

            taskItem.Name = name;
            taskItem.Description = description;
            taskItem.Type = type;
            taskItem.IsCompleted = isCompleted;
            taskItem.DeadLine = deadLine;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskById(int id)
        {
            var taskItem = await _dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id);
            if (taskItem == null)
            {
                _logger.LogInformation("User tried to delete non-existent task");
                throw new NullReferenceException();
            }

            _dbContext.Tasks.Remove(taskItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
