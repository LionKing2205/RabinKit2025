using RabinKit.Core.Entities;
using RabinKit.Database.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabinKit.Database.Configurations;

internal class TaskComponentsConfiguration : EntityBaseConfiguration<TaskComponent>
{
    /// <inheritdoc />
    public override void ConfigureChild(EntityTypeBuilder<TaskComponent> builder)
    {
        builder.Property(x => x.Toolbox)
            .HasJsonConversion();

        builder.Property(x => x.Playground);

        builder.HasMany(x => x.Attempts)
            .WithOne(x => x.TaskComponents)
            .HasForeignKey(x => x.TaskId)
            .HasPrincipalKey(x => x.Id);

        builder.HasMany(x => x.Values)
            .WithOne(x => x.Task)
            .HasForeignKey(x => x.TaskId)
            .HasPrincipalKey(x => x.Id);

        builder.HasMany(x => x.StatusR)
            .WithOne(x => x.TaskComponents)
            .HasForeignKey(x => x.TaskId)
             .HasPrincipalKey(x => x.Id);
    }
}