using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Abstractions;
using TaskManagerApi.Models;

namespace TaskManagerApi.Repository
{
    public class TaskManagerRepository : ITaskManagerRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public TaskManagerRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
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
            return await _dbContext.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(task => task.Id == id);
        }

        public async Task CreateTask(string name, string description, string type, bool isCompleted)
        {
            var taskItem = new TaskItem();
            taskItem.Name = name;
            taskItem.Description = description;
            taskItem.Type = type;
            taskItem.IsCompleted = isCompleted;
            await _dbContext.Tasks.AddAsync(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTaskById(int id, string name, string description, string type, bool isCompleted)
        {
            var taskItem = await _dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id)
                ?? throw new NullReferenceException();

            taskItem.Name = name;
            taskItem.Description = description;
            taskItem.Type = type;
            taskItem.IsCompleted = isCompleted;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskById(int id)
        {
            var taskitem = _dbContext.Tasks.FirstOrDefault(task => task.Id == id)
                ?? throw new NullReferenceException();

            _dbContext.Tasks.Remove(taskitem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
