namespace TaskManagerApi.Abstractions
{
    public interface IEmailService
    {
        Task SendMessage(string email, string message);
    }
}
