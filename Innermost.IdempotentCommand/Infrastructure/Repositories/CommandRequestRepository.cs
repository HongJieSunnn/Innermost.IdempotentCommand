using Innermost.IdempotentCommand.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand.Infrastructure.Repositories
{
    public class CommandRequestRepository : ICommandRequestRepository
    {
        private readonly IClientSessionHandle _session;
        private readonly IMongoCollection<CommandRequest> _commandRequest;
        public CommandRequestRepository(IClientSessionHandle session,IMongoDatabase database)
        {
            _session = session;
            _commandRequest = database.GetCollection<CommandRequest>("CommandRequests");
        }

        public async Task CreateCommandRequestRecordAsync<TCommand>(Guid guid)
        {
            if (Existed(guid))
                throw new ArgumentException($"CommandRequest with GUID ({guid}) has already existed.");

            var request = new CommandRequest(guid, typeof(TCommand).Name, DateTime.Now);
            await _commandRequest.InsertOneAsync(_session,request);
        }

        public bool Existed(Guid guid)
        {
            //FilterDefinition has a implict conversion by ExpressionFunc.So we should using the namespace of FilterDefinition or there will occur error.
            var existed = _commandRequest.FindSync<CommandRequest>(c =>c.ID==guid).Any();

            return existed;
        }

        public async Task<bool> ExistedAsync(Guid guid)
        {
            var existedTask = _commandRequest.FindAsync<CommandRequest>(c => c.ID == guid);

            var existed = await existedTask;

            return existed.Any();
        }
    }
}
