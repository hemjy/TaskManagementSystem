using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Core.Interfaces.Infrastructure.ExternalServices
{
    public interface IEmailService
    {
        Task Send(string from, string to, string body, string subject, List<string> copymails = null);
    }
}
