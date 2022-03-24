using Innermost.IdempotentCommand.Infrastructure.Repositories;
using Innermost.MongoDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        private static bool _added = false;
        public static IServiceCollection AddIdempotentCommandRequestStorage(this IServiceCollection services)
        {
            if (_added)
                throw new InvalidOperationException("IdempotentCommandRequestSQLStorage has already been added.");
            _added=true;

            services.AddSingleton<ICommandRequestRepository, CommandRequestRepository>(s =>
            {
                var context = s.GetRequiredService<MongoDBContextBase>();
                var database = context.Database;

                var session = s.GetRequiredService<IClientSessionHandle>();

                return new CommandRequestRepository(session,database);
            });

            return services;
        }

        public static IServiceCollection AddIdempotentCommandRequestSQLStorage<TDbContext>(this IServiceCollection services)
            where TDbContext:DbContext
        {
            if (_added)
                throw new InvalidOperationException("IdempotentCommandRequestStorage has already been added.");
            _added = true;

            services.AddSingleton<ICommandRequestRepository, CommandRequestRepository>(s =>
            {
                var context = s.GetRequiredService<MongoDBContextBase>();
                var database = context.Database;

                var session = s.GetRequiredService<IClientSessionHandle>();

                return new CommandRequestRepository(session, database);
            });

            return services;
        }
    }
}
