using TaskManagerApi.Models;

namespace TaskManagerApi.Abstractions
{
    public interface ITaskManagerRepository
    {
        Task<List<TaskItem>> Get();
        Task<TaskItem?> GetById(int id);
        Task CreateTask(string name, string description, string type, bool isCompleted, DateTime DeadLine);
        Task UpdateTaskById(int id, string name, string description, string type, bool isCompleted, DateTime DeadLine);
        Task DeleteTaskById(int id);
    }
}
