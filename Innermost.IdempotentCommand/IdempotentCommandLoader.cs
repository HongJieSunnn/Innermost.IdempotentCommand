using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand
{
    public sealed class IdempotentCommandLoader<TCommand, TResponse> :IRequest<TResponse>
        where TCommand:IRequest<TResponse>
    {
        public TCommand Command { get;private set; }
        public Guid ID { get;private set; }
        public IdempotentCommandLoader(TCommand command,Guid guid)
        {
            Command = command;
            ID = guid;
        }
    }
}
