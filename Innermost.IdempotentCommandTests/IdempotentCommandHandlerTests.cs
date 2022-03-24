using Autofac;
using Innermost.IdempotentCommand.Infrastructure.Repositories;
using Innermost.IdempotentCommandTests;
using Innermost.MongoDBContext.Configurations;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innermost.IdempotentCommand.Tests
{
    [TestClass()]
    public class IdempotentCommandHandlerTests
    {
        [TestMethod()]
        public async Task HandleTest()
        {
            var testCommand = new TestCommand();
            var guid = Guid.NewGuid();
            //Tip:Do not inherit IdempotentCommandBase or Autofac will not resolve the handler of derived class.Because IdempotentHandler handle IdempotentCommandBase type instead of the type inherit IdempotentCommandBase.
            //So if we inherit IdempotentCommandBase and send that command by mediator will throw NotFoundAppropriateType exception.
            //But IdempotentHandler should be inherited.
            IdempotentCommandLoader<TestCommand, string> testIdempotentComamnd = new IdempotentCommandLoader<TestCommand, string>(testCommand, guid);
            IdempotentCommandMongoDBContext context = new IdempotentCommandMongoDBContext(new MongoDBContextConfiguration<IdempotentCommandMongoDBContext>("mongodb://localhost:27017", "CommandRequestTest"));
            CommandRequestRepository commandRequestRepository = new CommandRequestRepository(context);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<MediatRModule>();
            containerBuilder.RegisterType<CommandRequestRepository>().As<ICommandRequestRepository>();
            containerBuilder.RegisterInstance<IdempotentCommandMongoDBContext>(context);
            var container = containerBuilder.Build();
            IMediator mediator = container.Resolve<IMediator>();

            var res = await mediator.Send(testIdempotentComamnd);
            var resExisted = await mediator.Send(testIdempotentComamnd);

            Assert.AreEqual("Test Command Handle Successfully", res);
            Assert.AreEqual(string.Empty, resExisted);
        }
    }

    internal class TestCommand : IRequest<string>
    {

    }

    internal class TestCommandHandler : IRequestHandler<TestCommand, string>
    {
        public async Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return "Test Command Handle Successfully";
        }
    }

    internal class IdempotentTestCommandHandler : IdempotentCommandHandler<TestCommand, string>
    {
        public IdempotentTestCommandHandler(IMediator mediator, ICommandRequestRepository commandRequestRepository) : base(mediator, commandRequestRepository)
        {

        }

        protected override string CreateDefault()
        {
            return string.Empty;
        }
    }
}