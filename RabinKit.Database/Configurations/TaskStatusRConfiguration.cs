using RabinKit.Core.Entities;
using RabinKit.Database.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabinKit.Database.Configurations;
internal class TaskStatusRConfiguration : EntityBaseConfiguration<TaskStatusR>
{
    /// <inheritdoc />
    public override void ConfigureChild(EntityTypeBuilder<TaskStatusR> builder)
    {

        builder.HasOne(x => x.TaskComponents)
            .WithMany(x => x.StatusR)
            .HasForeignKey(x => x.TaskId)
            .HasPrincipalKey(x => x.Id);

        builder.SetPropertyAccessModeField(x => x.TaskComponents, TaskStatusR.TaskComponentField);
    }
}