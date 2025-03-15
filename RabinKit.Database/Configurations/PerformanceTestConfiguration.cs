using RabinKit.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RabinKit.Database.Configurations;

/// <summary>
/// 
/// </summary>
internal class PerformanceTestConfiguration : EntityBaseConfiguration<PerformanceTest>
{
    /// <inheritdoc />
    public override void ConfigureChild(EntityTypeBuilder<PerformanceTest> builder)
    {
    }
}