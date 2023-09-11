using TaskManagementSystem.Core.Interfaces.Infrastructure.ExternalServices;

namespace TaskManagementSystem.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        public async Task Send(string from, string to, string body, string subect, List<string> copymails = null)
        {
            await Task.CompletedTask;
        }
    }
}
