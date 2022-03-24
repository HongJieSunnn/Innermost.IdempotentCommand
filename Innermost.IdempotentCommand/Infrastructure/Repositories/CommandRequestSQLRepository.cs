using Innermost.IdempotentCommand.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand.Infrastructure.Repositories
{
    public class CommandRequestSQLRepository<TDbContext> : ICommandRequestRepository
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        public CommandRequestSQLRepository(TDbContext context)
        {
            _context=context;
        }
        public async Task CreateCommandRequestRecordAsync<TCommand>(Guid guid)
        {
            if (Existed(guid))
                throw new ArgumentException($"CommandRequest with GUID ({guid}) has already existed.");

            var request = new CommandRequest(guid, typeof(TCommand).Name, DateTime.Now);
            _context.Add(request);
            await _context.SaveChangesAsync();
        }

        public bool Existed(Guid guid)
        {
            var commandRequest = _context.Find<CommandRequest>(("ID",guid));
            return commandRequest is not null;
        }

        public async Task<bool> ExistedAsync(Guid guid)
        {
            var commandRequest =await _context.FindAsync<CommandRequest>(("ID", guid));
            return commandRequest is not null;
        }
    }
}
