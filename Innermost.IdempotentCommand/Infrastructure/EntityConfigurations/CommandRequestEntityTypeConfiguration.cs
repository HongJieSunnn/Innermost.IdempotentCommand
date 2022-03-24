using Innermost.IdempotentCommand.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Innermost.IdempotentCommand.Infrastructure.EntityConfigurations
{
    public class CommandRequestEntityTypeConfiguration : IEntityTypeConfiguration<CommandRequest>
    {
        public void Configure(EntityTypeBuilder<CommandRequest> builder)
        {
            builder.ToTable("CommandRequests");

            builder.HasKey(c => c.ID);

            builder.Property(c=>c.CommandName).IsRequired();

            builder.Property(c=>c.Time).HasColumnType("DateTime").IsRequired();
        }
    }
}
