using RabinKit.Core.Entities;
using RabinKit.Database.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabinKit.Database.Configurations;

internal class TaskAttemptConfiguration : EntityBaseConfiguration<TaskAttempt>
{
    /// <inheritdoc />
    public override void ConfigureChild(EntityTypeBuilder<TaskAttempt> builder)
    {
        builder.Property(x => x.Inputs)
            .HasJsonConversion();

        builder.HasOne(x => x.TaskComponents)
            .WithMany(x => x.Attempts)
            .HasForeignKey(x => x.TaskId)
            .HasPrincipalKey(x => x.Id);

        builder.SetPropertyAccessModeField(x => x.TaskComponents, TaskAttempt.TaskComponentField);
    }
}