using TaskManagerApi.Abstractions;
using TaskManagerApi.Repository;

namespace TaskManagerApi.Services
{
    public class DeadlineCheckerService
    {
        private readonly ITaskManagerRepository _dbContext;

        public DeadlineCheckerService(ITaskManagerRepository dbContext
            IEmailService emailService)
        {
            LoadTasksAsync();
        }

        public 
    }
}
