using Innermost.IdempotentCommand.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand
{
    public class IdempotentCommandHandler<TCommand, TResponse> : IRequestHandler<IdempotentCommandLoader<TCommand, TResponse>, TResponse>
        where TCommand:IRequest<TResponse>
    {
        private readonly IMediator _mediator;
        private readonly ICommandRequestRepository _commandRequestRepository;
        public IdempotentCommandHandler(IMediator mediator,ICommandRequestRepository commandRequestRepository)
        {
            _mediator = mediator;
            _commandRequestRepository = commandRequestRepository;
        }

        public async Task<TResponse> Handle(IdempotentCommandLoader<TCommand, TResponse> request, CancellationToken cancellationToken)
        {
            if(_commandRequestRepository.Existed(request.ID))
            {
                await Task.CompletedTask;
                return CreateDefault();
            }

            await _commandRequestRepository.CreateCommandRequestRecordAsync<TCommand>(request.ID);
            var command = request.Command;
            var response =await _mediator.Send<TResponse>(command??throw new ArgumentNullException(nameof(request.Command)), cancellationToken);

            return response;
        }

        protected virtual TResponse CreateDefault()
        {
            return default(TResponse);
        }
    }
}
