using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options)
        {

        }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
