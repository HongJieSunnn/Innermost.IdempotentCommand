using Microsoft.VisualStudio.TestTools.UnitTesting;
using Innermost.IdempotentCommand.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innermost.IdempotentCommand.Infrastructure.DBContext;
using Innermost.MongoDBContext.Configurations;

namespace Innermost.IdempotentCommand.Infrastructure.Repositories.Tests
{
    [TestClass()]
    public class CommandRequestRepositoryTests
    {
        [TestMethod()]
        public async Task CreateCommandRequestRecordAsyncTest()
        {
            IdempotentCommandMongoDBContext context = new IdempotentCommandMongoDBContext(new MongoDBContextConfiguration<IdempotentCommandMongoDBContext>("mongodb://localhost:27017", "CommandRequestTest"));
            CommandRequestRepository commandRequestRepository = new CommandRequestRepository(context);
            var guid = Guid.NewGuid();

            await commandRequestRepository.CreateCommandRequestRecordAsync<TestCommand>(guid);
            var commandExisted = commandRequestRepository.Existed(guid);  

            Assert.IsTrue(commandExisted);
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await commandRequestRepository.CreateCommandRequestRecordAsync<TestCommand>(guid));
        }

        [TestMethod()]
        public async Task ExistedTest()
        {
            IdempotentCommandMongoDBContext context = new IdempotentCommandMongoDBContext(new MongoDBContextConfiguration<IdempotentCommandMongoDBContext>("mongodb://localhost:27017", "CommandRequestTest"));
            CommandRequestRepository commandRequestRepository = new CommandRequestRepository(context);
            var guid = Guid.NewGuid();

            var firstTime = commandRequestRepository.Existed(guid);
            await commandRequestRepository.CreateCommandRequestRecordAsync<TestCommand>(guid);
            var secondTime = commandRequestRepository.Existed(guid);

            Assert.IsFalse(firstTime);
            Assert.IsTrue(secondTime);
        }

        [TestMethod()]
        public async Task ExistedAsyncTest()
        {
            IdempotentCommandMongoDBContext context = new IdempotentCommandMongoDBContext(new MongoDBContextConfiguration<IdempotentCommandMongoDBContext>("mongodb://localhost:27017", "CommandRequestTest"));
            CommandRequestRepository commandRequestRepository = new CommandRequestRepository(context);
            var guid = Guid.NewGuid();

            var firstTime =await commandRequestRepository.ExistedAsync(guid);
            await commandRequestRepository.CreateCommandRequestRecordAsync<TestCommand>(guid);
            var secondTime = await commandRequestRepository.ExistedAsync(guid);

            Assert.IsFalse(firstTime);
            Assert.IsTrue(secondTime);
        }
    }

    internal class TestCommand
    {

    }
}
