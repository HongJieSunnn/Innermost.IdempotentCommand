using Innermost.IdempotentCommand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand.Infrastructure.Repositories
{
    public interface ICommandRequestRepository
    {
        bool Existed(Guid guid);
        Task<bool> ExistedAsync(Guid guid);
        Task CreateCommandRequestRecordAsync<TCommand>(Guid guid);
    }
}
