using TaskManagerApi.Abstractions;
using TaskManagerApi.Repository;

namespace TaskManagerApi.Services
{
    public class DeadlineCheckerService
    {
        private readonly ITaskManagerRepository _dbContext;
        private readonly IEmailService _emailService;

        public DeadlineCheckerService(ITaskManagerRepository dbContext,
            IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task CheckOneDayDeadline()
        {
            var tasks = await _dbContext.Get();
            var tasksToNotify = tasks.Where(task =>
            task.DeadLine.HasValue && 
            task.DeadLine.Value.Date == DateTime.Today.AddDays(1)).ToList();

            foreach ( var task in tasksToNotify)
            {
                await _emailService.SendMessage("someEmail@serv.com", "Some message");
            }
        }
    }
}
