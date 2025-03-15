using RabinKit.Core.Entities;
using RabinKit.Database.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabinKit.Database.Configurations;

/// <summary>
/// 
/// </summary>
internal class PerformanceTestAttemptConfiguration : EntityBaseConfiguration<PerformanceTestAttempt>
{
    /// <inheritdoc />
    public override void ConfigureChild(EntityTypeBuilder<PerformanceTestAttempt> builder)
    {
        builder.Property(x => x.Runs)
            .HasJsonConversion();
    }
}