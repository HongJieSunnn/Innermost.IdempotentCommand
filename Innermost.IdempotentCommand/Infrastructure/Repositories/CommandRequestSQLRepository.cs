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
        public async Task CreateCommandRequestRecordAsync<TCommand>(Guid id)
        {
            if (Existed(id))
                throw new ArgumentException($"CommandRequest with GUID ({id}) has already existed.");

            var request = new CommandRequest(id, typeof(TCommand).Name, DateTime.Now);
            _context.Add(request);
            await _context.SaveChangesAsync();
        }

        public bool Existed(Guid id)
        {
            var commandRequest = _context.Find<CommandRequest>(id);
            return commandRequest is not null;
        }

        public async Task<bool> ExistedAsync(Guid id)
        {
            var commandRequest =await _context.FindAsync<CommandRequest>(id);
            return commandRequest is not null;
        }
    }
}
