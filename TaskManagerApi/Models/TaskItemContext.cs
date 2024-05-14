using Microsoft.EntityFrameworkCore;

namespace TaskManagerApi.Models
{
    public class TaskItemContext : DbContext
    {
        public TaskItemContext(DbContextOptions<TaskItemContext> options)
            : base(options)
        {

        }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
