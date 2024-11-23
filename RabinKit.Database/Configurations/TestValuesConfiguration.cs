using RabinKit.Core.Entities;
using RabinKit.Database.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace RabinKit.Database.Configurations 
{
    internal class TestValuesConfiguration : EntityBaseConfiguration<TestValue>
    {
    public override void ConfigureChild(EntityTypeBuilder<TestValue> builder)
    {
        builder.Property(x => x.InputVars).HasJsonConversion();
        builder.Property(x => x.OutputVars).HasJsonConversion();

        builder.Property(x => x.TaskId);

            builder.HasOne(x => x.Task)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.TaskId)
                .HasPrincipalKey(x => x.Id);

            //builder.HasMany(x => x.Attempts)
            //    .WithOne(x => x.Test)
            //    .HasForeignKey(x => x.TestId)
            //    .HasPrincipalKey(x => x.Id);

            builder.SetPropertyAccessModeField(x => x.Task, TestValue.TaskField);
        }
}
}
