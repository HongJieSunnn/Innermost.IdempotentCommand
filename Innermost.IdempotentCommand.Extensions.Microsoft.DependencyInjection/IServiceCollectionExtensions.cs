using Innermost.IdempotentCommand.Infrastructure.Repositories;
using Innermost.MongoDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Innermost.IdempotentCommand.Extensions.Microsoft.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIdempotentCommandRequestStorage(this IServiceCollection services)
        {
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
            services.AddScoped<ICommandRequestRepository, CommandRequestSQLRepository<TDbContext>>();

            return services;
        }
    }
}
